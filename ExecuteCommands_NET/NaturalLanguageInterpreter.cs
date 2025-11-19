using System;
using System.Collections.Generic;
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;

namespace ExecuteCommands
{
    public class NaturalLanguageInterpreter
    {
            // Missing action types
            public record CloseTabAction : ActionBase { }
            public record SetWindowAlwaysOnTopAction(string? Application) : ActionBase;

            // P/Invoke for SetWindowPos
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            // P/Invoke for ShowWindow
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            // Stub for InterpretAsync
            public System.Threading.Tasks.Task<ActionBase?> InterpretAsync(string text)
            {
                text = (text ?? string.Empty).ToLowerInvariant().Trim();
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync input: {text}\n");
                // Maximize/full screen window (robust match for any phrase)
                if ((text.Contains("maximize") || text.Contains("full screen")) && text.Contains("window"))
                {
                    var action = new MoveWindowAction(
                        Target: "active",
                        Monitor: "current",
                        Position: "center",
                        WidthPercent: 100,
                        HeightPercent: 100
                    );
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name}\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // ...existing code...
                // TODO: Add more rules for other actions
                System.IO.File.AppendAllText("app.log", "[DEBUG] InterpretAsync: No match, returning null\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(null);
                }
        // Supported apps for close tab
        private static readonly string[] SupportedCloseTabApps = new[] { "chrome", "msedge", "firefox", "brave", "opera", "code", "devenv" };

        public string ExecuteActionAsync(ActionBase action)
        {
            System.IO.File.AppendAllText("app.log", $"[DEBUG] ExecuteActionAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
            if (action is MoveWindowAction move)
            {
                // Get active window handle
                IntPtr hWnd = Commands.GetForegroundWindow();
                if (hWnd == IntPtr.Zero)
                    return "No active window found.";
                // Maximize logic
                if ((move.Position == "center" || move.Position == null) && move.WidthPercent == 100 && move.HeightPercent == 100)
                {
                    // Maximize window
                    const int SW_MAXIMIZE = 3;
                    bool success = ShowWindow(hWnd, SW_MAXIMIZE);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to maximize window. Win32 error: {error}\n");
                        return $"Failed to maximize window. Win32 error: {error}";
                    }
                    return "Window maximized.";
                }
                // TODO: Implement other window move actions (left/right/next monitor)
                return "[Stub] Window move not implemented for: " + move.ToString();
            }
            else if (action is CloseTabAction)
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
            else if (action is SetWindowAlwaysOnTopAction setTop)
            {
                IntPtr hWnd = IntPtr.Zero;
                string appName = setTop.Application?.ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(appName))
                {
                    var procs = System.Diagnostics.Process.GetProcessesByName(appName);
                    if (procs.Length > 0)
                        hWnd = procs[0].MainWindowHandle;
                }
                else
                {
                    hWnd = Commands.GetForegroundWindow();
                }
                if (hWnd == IntPtr.Zero)
                    return "No window found to set always on top.";
                IntPtr HWND_TOPMOST = new IntPtr(-1);
                const uint SWP_NOMOVE = 0x0002;
                const uint SWP_NOSIZE = 0x0001;
                const uint SWP_SHOWWINDOW = 0x0040;
                bool success = SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                if (!success)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    System.IO.File.AppendAllText("app.log", $"Failed to set window always on top. Win32 error: {error}\n");
                    return $"Failed to set window always on top. Win32 error: {error}";
                }
                return "Window set to always on top.";
            }
            else if (action is OpenFolderAction folder)
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
            else if (action is ShowHelpAction)
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
            else if (action is LaunchAppAction app)
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
            else if (action is SendKeysAction keys)
            {
                try
                {
                    var sim = new WindowsInput.InputSimulator();
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
                                if (Enum.TryParse<WindowsInput.Native.VirtualKeyCode>("VK_" + part.ToUpper(), out var vk))
                                    mainKeys.Add(vk);
                                else if (part.Length == 1 && char.IsLetterOrDigit(part[0]))
                                    mainKeys.Add((WindowsInput.Native.VirtualKeyCode)Enum.Parse(typeof(WindowsInput.Native.VirtualKeyCode), "VK_" + part.ToUpper()));
                                else if (part.StartsWith("f") && int.TryParse(part.Substring(1), out int fnum) && fnum >= 1 && fnum <= 24)
                                    mainKeys.Add((WindowsInput.Native.VirtualKeyCode)Enum.Parse(typeof(WindowsInput.Native.VirtualKeyCode), "F" + fnum));
                                break;
                        }
                    }
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
            else
            {
                return "Unknown action type.";
            }
        }

        public string HandleNaturalAsync(string text)
        {
            var actionTask = InterpretAsync(text);
            actionTask.Wait();
            var action = actionTask.Result;
            System.IO.File.AppendAllText("app.log", $"[DEBUG] HandleNaturalAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
            if (action == null)
            {
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }
    }
}
