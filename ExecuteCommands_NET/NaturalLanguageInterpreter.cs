    /// <summary>
    /// Action for showing help/command list.
    /// </summary>
using System;
using ExecuteCommands;

namespace ExecuteCommands
{
    /// <summary>
    /// Handles natural language interpretation and action execution.
    /// </summary>
    public class NaturalLanguageInterpreter
    {
        // Supported apps for close tab
        private static readonly string[] SupportedCloseTabApps = new[] { "chrome", "msedge", "firefox", "brave", "opera", "code", "devenv" };
        public record CloseTabAction : ActionBase { }
        public record FocusAppAction(string AppName) : ActionBase { }
        // P/Invoke for window management
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Interprets the input text and returns an ActionBase (or null if no match)
        /// </summary>
        public ActionBase? InterpretAsync(string text)
        {
            string t = text.ToLowerInvariant();
            // Mitigate speech misrecognition: 'clothes', 'clothed', etc. as 'close tab'
            if (t.Contains("close tab") || t.Contains("clothes") || t.Contains("clothed") || t.Contains("close the tab"))
                return new CloseTabAction();

            // Focus/switch/activate app intent
            var focusWords = new[] { "switch to ", "focus ", "activate " };
            foreach (var word in focusWords)
            {
                if (t.StartsWith(word))
                {
                    var app = t.Substring(word.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(app))
                        return new FocusAppAction(app);
                }
            }
            // Send keys
            if (t.StartsWith("press "))
            {
                var keysText = t.Substring(6).Trim();
                if (!string.IsNullOrWhiteSpace(keysText))
                    return new SendKeysAction(keysText);
            }
            // Help/introspection
            if (t.Contains("what can i say") || t.Contains("help") || t.Contains("commands") || t.Contains("what are the commands"))
                return new ShowHelpAction();

            // Folder opening
            if ((t.Contains("open") || t.Contains("show")) && (t.Contains("downloads") || t.Contains("download folder")))
                return new OpenFolderAction("Downloads");
            if ((t.Contains("open") || t.Contains("show")) && (t.Contains("documents") || t.Contains("document folder")))
                return new OpenFolderAction("Documents");


            // Window management - flexible matching
            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("right") || t.Contains("to the right") || t.Contains("on the right")))
                return new MoveWindowAction("active", "current", "right", 50, 100);

            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("left") || t.Contains("to the left") || t.Contains("on the left")))
                return new MoveWindowAction("active", "current", "left", 50, 100);

            if (t.Contains("window") && (t.Contains("full screen") || t.Contains("maximize") || t.Contains("center")))
                return new MoveWindowAction("active", "current", "center", 100, 100);

            // More flexible matching for moving window to another monitor
            if ((t.Contains("move") || t.Contains("put") || t.Contains("send") || t.Contains("shift")) && t.Contains("window") && (t.Contains("other monitor") || t.Contains("next monitor") || t.Contains("second monitor") || t.Contains("another monitor") || t.Contains("other screen") || t.Contains("next screen") || t.Contains("second screen") || t.Contains("another screen") || t.Contains("my other monitor") || t.Contains("my other screen")))
                return new MoveWindowAction("active", "next", null, null, null);

                // App launch
                if ((t.Contains("open") || t.Contains("launch") || t.Contains("start")))
                {
                    // Map common app names to executables
                    if (t.Contains("edge") || t.Contains("microsoft edge"))
                        return new LaunchAppAction("msedge.exe");
                    if (t.Contains("chrome"))
                        return new LaunchAppAction("chrome.exe");
                    if (t.Contains("visual studio code") || t.Contains("code"))
                        return new LaunchAppAction("code.exe");
                    if (t.Contains("visual studio"))
                        return new LaunchAppAction("devenv.exe");
                    if (t.Contains("outlook"))
                        return new LaunchAppAction("outlook.exe");
                    // Fallback: try to extract app name after 'open'
                    var openIdx = t.IndexOf("open ");
                    if (openIdx >= 0)
                    {
                        var appName = t.Substring(openIdx + 5).Trim();
                        if (!string.IsNullOrWhiteSpace(appName))
                            return new LaunchAppAction(appName);
                    }
                }
            // Fallback: no match
            return null;
        }

        /// <summary>
        /// Executes the given action. For now, just returns a string describing the action.
        /// </summary>
        public string ExecuteActionAsync(ActionBase action)
        {
            if (action is CloseTabAction)
            {
                string procName = CurrentApplicationHelper.GetCurrentProcessName();
                if (string.IsNullOrEmpty(procName))
                    return "Could not detect current application.";
                if (SupportedCloseTabApps.Contains(procName))
                {
                    try
                    {
                        var sim = new WindowsInput.InputSimulator();
                        sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_W);
                        return $"Sent Ctrl+W to {procName} (close tab).";
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.AppendAllText("app.log", $"Failed to send Ctrl+W to {procName}: {ex.Message}\n");
                        return $"Failed to send Ctrl+W to {procName}: {ex.Message}";
                    }
                }
                else
                {
                    return $"Current app '{procName}' is not supported for 'close tab'.";
                }
            }
            try
            {
                switch (action)
                {
                    case MoveWindowAction move:
                        {
                            var hWnd = Commands.GetForegroundWindow();
                            if (hWnd == IntPtr.Zero)
                                return "No active window found.";

                            if (move.Position == "center" && move.WidthPercent == 100 && move.HeightPercent == 100)
                            {
                                Commands.ShowCursor(true);
                                ShowWindow(hWnd, 3);
                                return "Window maximized.";
                            }

                            if ((move.Position == "left" || move.Position == "right") && move.WidthPercent == 50 && move.HeightPercent == 100)
                            {
                                var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
                                if (primaryScreen == null)
                                    return "No primary screen detected.";
                                var screen = primaryScreen.WorkingArea;
                                int width = screen.Width / 2;
                                int height = screen.Height;
                                int x = move.Position == "left" ? screen.Left : screen.Left + width;
                                int y = screen.Top;
                                bool success = SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040);
                                if (!success)
                                {
                                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                                    System.IO.File.AppendAllText("app.log", $"Failed to move window. Win32 error: {error}\n");
                                    return $"Failed to move window. Win32 error: {error}";
                                }
                                return $"Window moved to {move.Position} half.";
                            }

                            // Implement moving window to next monitor (handle nulls)
                            if ((move.Monitor == "next") || (move.Position == null && move.WidthPercent == null && move.HeightPercent == null && (move.Target == "active")))
                            {
                                var screens = System.Windows.Forms.Screen.AllScreens;
                                if (screens.Length < 2)
                                    return "Only one monitor detected.";

                                // Restore window if minimized
                                const int SW_RESTORE = 9;
                                ShowWindow(hWnd, SW_RESTORE);

                                // Get current window position
                                var currentScreen = System.Windows.Forms.Screen.FromHandle(hWnd);
                                int currentIndex = Array.IndexOf(screens, currentScreen);
                                int nextIndex = (currentIndex + 1) % screens.Length;
                                var nextScreen = screens[nextIndex].WorkingArea;

                                // Move window to next screen, maximize
                                bool success = SetWindowPos(hWnd, IntPtr.Zero, nextScreen.Left, nextScreen.Top, nextScreen.Width, nextScreen.Height, 0x0040);
                                if (!success)
                                {
                                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                                    System.IO.File.AppendAllText("app.log", $"Failed to move window to next monitor. Win32 error: {error}\n");
                                    return $"Failed to move window to next monitor. Win32 error: {error}";
                                }
                                SetForegroundWindow(hWnd);
                                return "Window moved to next monitor.";
                            }

                            return $"[Stub] Window move not implemented for: {move}";
                        }
                    case OpenFolderAction folder:
                        {
                            string path;
                            switch (folder.KnownFolder.ToLower())
                            {
                                case "downloads":
                                    path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                                    break;
                                case "documents":
                                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                    break;
                                default:
                                    path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                    break;
                            }
                            var psi = new System.Diagnostics.ProcessStartInfo("explorer.exe", path)
                            {
                                UseShellExecute = true
                            };
                            System.Diagnostics.Process.Start(psi);
                            return $"Opened folder: {folder.KnownFolder} ({path})";
                        }
                    case ShowHelpAction:
                        {
                            string helpText = "You may solemnly say:\n" +
                                "- Move this window to the left/right\n" +
                                "- Maximize this window\n" +
                                "- Open downloads/documents\n" +
                                "- Move window to other screen\n" +
                                "- (More natural commands can be added)";
                            TrayNotificationHelper.ShowNotification("ExecuteCommands.NET Help", helpText, 7000);
                            return helpText;
                        }
                        case LaunchAppAction app:
                            {
                                try
                                {
                                    var psi = new System.Diagnostics.ProcessStartInfo(app.AppIdOrPath)
                                    {
                                        UseShellExecute = true
                                    };
                                    System.Diagnostics.Process.Start(psi);
                                    return $"Launched app: {app.AppIdOrPath}";
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText("app.log", $"Failed to launch app: {app.AppIdOrPath}. Error: {ex.Message}\n");
                                    return $"Failed to launch app: {app.AppIdOrPath}. Error: {ex.Message}";
                                }
                            }
                        case SendKeysAction keys:
                            {
                                try
                                {
                                    // Use InputSimulator to send keys
                                    var sim = new WindowsInput.InputSimulator();
                                    // Simple parser: split by space, handle modifiers
                                    var keyParts = keys.KeysText.Split(' ');
                                    var modifiers = new List<WindowsInput.Native.VirtualKeyCode>();
                                    var mainKeys = new List<WindowsInput.Native.VirtualKeyCode>();
                                    foreach (var part in keyParts)
                                    {
                                        switch (part)
                                        {
                                            case "control":
                                            case "ctrl":
                                                modifiers.Add(WindowsInput.Native.VirtualKeyCode.CONTROL);
                                                break;
                                            case "shift":
                                                modifiers.Add(WindowsInput.Native.VirtualKeyCode.SHIFT);
                                                break;
                                            case "alt":
                                                modifiers.Add(WindowsInput.Native.VirtualKeyCode.MENU);
                                                break;
                                            case "windows":
                                            case "win":
                                                modifiers.Add(WindowsInput.Native.VirtualKeyCode.LWIN);
                                                break;
                                            default:
                                                // Try to parse as key
                                                if (Enum.TryParse<WindowsInput.Native.VirtualKeyCode>("VK_" + part.ToUpper(), out var vk))
                                                    mainKeys.Add(vk);
                                                else if (part.Length == 1 && char.IsLetterOrDigit(part[0]))
                                                    mainKeys.Add((WindowsInput.Native.VirtualKeyCode)Enum.Parse(typeof(WindowsInput.Native.VirtualKeyCode), "VK_" + part.ToUpper()));
                                                else if (part.StartsWith("f") && int.TryParse(part.Substring(1), out int fnum) && fnum >= 1 && fnum <= 24)
                                                    mainKeys.Add((WindowsInput.Native.VirtualKeyCode)Enum.Parse(typeof(WindowsInput.Native.VirtualKeyCode), "F" + fnum));
                                                // else ignore
                                                break;
                                        }
                                    }
                                    // Send key combination
                                    if (mainKeys.Count > 0)
                                    {
                                        sim.Keyboard.ModifiedKeyStroke(modifiers, mainKeys);
                                        return $"Sent keys: {keys.KeysText}";
                                    }
                                    else if (modifiers.Count > 0)
                                    {
                                        sim.Keyboard.KeyDown(modifiers[0]);
                                        sim.Keyboard.KeyUp(modifiers[0]);
                                        return $"Sent modifier key: {modifiers[0]}";
                                    }
                                    return $"No valid keys found in: {keys.KeysText}";
                                }
                                catch (Exception ex)
                                {
                                    System.IO.File.AppendAllText("app.log", $"Failed to send keys: {keys.KeysText}. Error: {ex.Message}\n");
                                    return $"Failed to send keys: {keys.KeysText}. Error: {ex.Message}";
                                }
                            }
                    default:
                        return "Unknown action type.";
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("app.log", $"Error executing action: {ex.Message}\n");
                return $"Error executing action: {ex.Message}";
            }
        }

        public string HandleNaturalAsync(string text)
        {
            var action = InterpretAsync(text);
            if (action == null)
            {
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }
    }
}
