    /// <summary>
    /// Action for showing help/command list.
    /// </summary>
using System;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
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
            /// Uses OpenAI API to interpret text and return an ActionBase (AI fallback).
            /// </summary>
            public async Task<ActionBase?> InterpretWithAIAsync(string text)
            {
                // Read API key from environment variable
                string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    System.IO.File.AppendAllText("app.log", "OPENAI_API_KEY environment variable not set.\n");
                    return null;
                }
                // Set default model name
                string modelName = "gpt-4.1";

                // Read prompt from markdown file
                string promptPath = "openai_prompt.md";
                string prompt;
                try
                {
                    prompt = System.IO.File.ReadAllText(promptPath);
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText("app.log", $"Failed to read {promptPath}: {ex.Message}\nUsing default prompt.\n");
                    prompt = "You are an assistant that interprets natural language commands for Windows automation. Output a JSON object for the closest matching action.";
                }

                System.IO.File.AppendAllText("app.log", $"[AI] Fallback triggered for: {text}\n");
                try
                {
                    var chatClient = new ChatClient("gpt-4o", apiKey);
                    var messages = new List<ChatMessage>
                    {
                        new SystemChatMessage(prompt),
                        new UserChatMessage(text)
                    };
                    var completionResult = await chatClient.CompleteChatAsync(messages);
                    var completion = completionResult.Value;
                    var message = completion.Content[0].Text;
                    System.IO.File.AppendAllText("app.log", $"[AI] Raw response: {message}\n");
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        try
                        {
                            var json = System.Text.Json.JsonDocument.Parse(message);
                            var root = json.RootElement;
                            if (root.TryGetProperty("type", out var typeProp))
                            {
                                string type = typeProp.GetString() ?? "";
                                switch (type)
                                {
                                    case "MoveWindowAction":
                                        return new MoveWindowAction(
                                            root.GetProperty("Target").GetString() ?? "active",
                                            root.GetProperty("Monitor").GetString() ?? "current",
                                            root.GetProperty("Position").GetString(),
                                            root.TryGetProperty("WidthPercent", out var wp) ? wp.GetInt32() : (int?)null,
                                            root.TryGetProperty("HeightPercent", out var hp) ? hp.GetInt32() : (int?)null
                                        );
                                    case "LaunchAppAction":
                                        return new LaunchAppAction(root.GetProperty("AppIdOrPath").GetString() ?? "");
                                    case "SendKeysAction":
                                        return new SendKeysAction(root.GetProperty("KeysText").GetString() ?? "");
                                    case "OpenFolderAction":
                                        return new OpenFolderAction(root.GetProperty("KnownFolder").GetString() ?? "");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.IO.File.AppendAllText("app.log", $"Failed to parse OpenAI response: {ex.Message}\nResponse: {message}\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText("app.log", $"[AI] OpenAI API call failed: {ex.Message}\n");
                }
                return null;
            }

        /// <summary>
        /// Interprets the input text and returns an ActionBase (rule-based, then AI fallback)
        /// </summary>
        public async Task<ActionBase?> InterpretAsync(string text)
        {
            string t = text.ToLowerInvariant();
            // Rule-based matching
            if (t.Contains("close tab") || t.Contains("clothes") || t.Contains("clothed") || t.Contains("close the tab"))
                return new CloseTabAction();

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
            if (t.StartsWith("press "))
            {
                var keysText = t.Substring(6).Trim();
                if (!string.IsNullOrWhiteSpace(keysText))
                    return new SendKeysAction(keysText);
            }
            if (t.Contains("what can i say") || t.Contains("help") || t.Contains("commands") || t.Contains("what are the commands"))
                return new ShowHelpAction();

            if ((t.Contains("open") || t.Contains("show")) && (t.Contains("downloads") || t.Contains("download folder")))
                return new OpenFolderAction("Downloads");
            if ((t.Contains("open") || t.Contains("show")) && (t.Contains("documents") || t.Contains("document folder")))
                return new OpenFolderAction("Documents");

            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("right") || t.Contains("to the right") || t.Contains("on the right")))
                return new MoveWindowAction("active", "current", "right", 50, 100);

            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("left") || t.Contains("to the left") || t.Contains("on the left")))
                return new MoveWindowAction("active", "current", "left", 50, 100);

            if (t.Contains("window") && (t.Contains("full screen") || t.Contains("maximize") || t.Contains("center")))
                return new MoveWindowAction("active", "current", "center", 100, 100);

            if ((t.Contains("move") || t.Contains("put") || t.Contains("send") || t.Contains("shift")) && t.Contains("window") && (t.Contains("other monitor") || t.Contains("next monitor") || t.Contains("second monitor") || t.Contains("another monitor") || t.Contains("other screen") || t.Contains("next screen") || t.Contains("second screen") || t.Contains("another screen") || t.Contains("my other monitor") || t.Contains("my other screen")))
                return new MoveWindowAction("active", "next", null, null, null);

            if ((t.Contains("open") || t.Contains("launch") || t.Contains("start")))
            {
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
                var openIdx = t.IndexOf("open ");
                if (openIdx >= 0)
                {
                    var appName = t.Substring(openIdx + 5).Trim();
                    if (!string.IsNullOrWhiteSpace(appName))
                        return new LaunchAppAction(appName);
                }
            }

            // Fallback: call AI if no match
            string? currentApp = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
            string aiInput = text;
            if (!string.IsNullOrWhiteSpace(currentApp))
            {
                aiInput += $"\nCurrentApplication: {currentApp}";
            }
            return await InterpretWithAIAsync(aiInput);
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
                        case FocusAppAction focus:
                            {
                                // Try to find running process
                                string appName = focus.AppName.ToLowerInvariant();
                                string[] knownApps = new[] { "msedge", "chrome", "firefox", "brave", "opera", "code", "devenv", "outlook" };
                                string exeName = appName;
                                if (appName == "edge" || appName == "microsoft edge") exeName = "msedge";
                                if (appName == "chrome") exeName = "chrome";
                                if (appName == "firefox") exeName = "firefox";
                                if (appName == "brave") exeName = "brave";
                                if (appName == "opera") exeName = "opera";
                                if (appName == "code" || appName == "visual studio code") exeName = "code";
                                if (appName == "visual studio") exeName = "devenv";
                                if (appName == "outlook") exeName = "outlook";
                                var procs = System.Diagnostics.Process.GetProcessesByName(exeName);
                                if (procs.Length > 0)
                                {
                                    // Bring first process window to foreground
                                    var proc = procs[0];
                                    IntPtr hWnd = proc.MainWindowHandle;
                                    if (hWnd != IntPtr.Zero)
                                    {
                                        SetForegroundWindow(hWnd);
                                        return $"Focused app: {exeName}";
                                    }
                                    else
                                    {
                                        return $"App '{exeName}' is running but has no main window.";
                                    }
                                }
                                else
                                {
                                    // Not running, launch
                                    try
                                    {
                                        var psi = new System.Diagnostics.ProcessStartInfo(exeName + ".exe")
                                        {
                                            UseShellExecute = true
                                        };
                                        System.Diagnostics.Process.Start(psi);
                                        return $"Launched app: {exeName}.exe";
                                    }
                                    catch (Exception ex)
                                    {
                                        System.IO.File.AppendAllText("app.log", $"Failed to launch app: {exeName}.exe. Error: {ex.Message}\n");
                                        return $"Failed to launch app: {exeName}.exe. Error: {ex.Message}";
                                    }
                                }
                            }
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
            var actionTask = InterpretAsync(text);
            actionTask.Wait();
            var action = actionTask.Result;
            if (action == null)
            {
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }
    }
}
