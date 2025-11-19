using System;
using System.Collections.Generic;
using System.Diagnostics;
using WindowsInput;
using WindowsInput.Native;

namespace ExecuteCommands
{
    using OpenAI;
    using OpenAI.Chat;
    using OpenAI.Models;
    public class NaturalLanguageInterpreter
    {
        /// <summary>
        /// Uses OpenAI API to interpret text and return an ActionBase (AI fallback).
        /// </summary>
        public async System.Threading.Tasks.Task<ActionBase?> InterpretWithAIAsync(string text)
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
                var chatClient = new ChatClient(modelName, apiKey);
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
        // ...existing code...
            // Helper to remove polite modifiers from input
            private static string RemovePoliteModifiers(string text)
            {
                var politeWords = new[] { "please", "could you", "would you", "can you", "may you", "kindly", "will you", "would you kindly" };
                foreach (var word in politeWords)
                {
                    text = text.Replace(word, "", StringComparison.InvariantCultureIgnoreCase);
                }
                return text.Trim();
            }

            private static string GetLogPath()
            {
                string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "app.log");
                return System.IO.Path.GetFullPath(logPath);
            }
        // Central list of available commands/actions for AI matching
        public static readonly List<(string Command, string Description)> AvailableCommands = new()
        {
            ("maximize window", "Maximize the active window"),
            ("move window to left half", "Move the active window to the left half of the screen"),
            ("move window to right half", "Move the active window to the right half of the screen"),
            ("move window to other monitor", "Move the active window to the next monitor"),
            ("set window always on top", "Set the active window to always be on top"),
            ("open downloads", "Open the Downloads folder"),
            ("open documents", "Open the Documents folder"),
            ("close tab", "Close the current tab in supported applications"),
            ("send keys", "Send a key sequence to the active window"),
            ("launch app", "Launch a specified application"),
            ("focus app", "Focus a specified application window"),
            ("show help", "Show help and available commands"),
        };
        // P/Invoke for MonitorFromWindow
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        // P/Invoke for GetMonitorInfo
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        // MONITORINFOEX struct
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public struct MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDevice;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
            // Missing action types
            public record CloseTabAction : ActionBase { }
            public record SetWindowAlwaysOnTopAction(string? Application) : ActionBase;

            // P/Invoke for SetWindowPos
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            // P/Invoke for ShowWindow
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            // InterpretAsync implementation
            public System.Threading.Tasks.Task<ActionBase?> InterpretAsync(string text)
            {
                text = (text ?? string.Empty).ToLowerInvariant().Trim();
                // Remove polite modifiers and extra punctuation
                text = RemovePoliteModifiers(text);
                text = text.Replace("  ", " ").Replace(".", "").Replace(",", "").Trim();
                // Remove extra words that often appear in these commands
                var extraWords = new[] { "of this", "of others", "of other windows", "on top of others", "on top of this" };
                foreach (var ew in extraWords) text = text.Replace(ew, "");
                text = text.Trim();
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync normalized input: {text}\n");

                // More robust matching for 'always on top'/'float above'/'restore' commands
                var alwaysOnTopPatterns = new[] {
                    "always on top", "on top", "float above", "float this window", "float above other windows",
                    "make this window float above", "make this window float", "float this window above",
                    "float window above", "make window float", "make window always on top",
                    "put this window on top", "put window on top", "make window float above", "put window above",
                    "float this window above other windows", "float window above other windows", "float window above others",
                    "float this window above others", "float window above",
                    "put this window above other windows", "put this window above others", "put window above other windows",
                    "put window above others", "make this window always on top", "make window always on top"
                };
                                // Restore window (un-maximize)
                                if ((text.Contains("restore") || text.Contains("unmaximize")) && text.Contains("window"))
                                {
                                    var action = new MoveWindowAction(
                                        Target: "active",
                                        Monitor: "current",
                                        Position: "center",
                                        WidthPercent: 80,
                                        HeightPercent: 80
                                    );
                                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (restore window)\n");
                                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                                }
                bool matchedAlwaysOnTop = false;
                foreach (var pattern in alwaysOnTopPatterns)
                {
                    if (text.Contains(pattern))
                    {
                        matchedAlwaysOnTop = true;
                        System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync matched pattern: {pattern}\n");
                        break;
                    }
                }
                // Also match regex variants like 'float.*window.*top' or 'make.*window.*top'
                if (!matchedAlwaysOnTop)
                {
                    var regexPatterns = new[] {
                        "float.*window.*top", "make.*window.*top", "float.*window.*above", "make.*window.*float", "put.*window.*top", "put.*window.*above"
                    };
                    foreach (var rx in regexPatterns)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(text, rx))
                        {
                            matchedAlwaysOnTop = true;
                            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync matched regex: {rx}\n");
                            break;
                        }
                    }
                    // Catch-all: match any phrase containing 'float', 'window', and 'above' in any order
                    if (!matchedAlwaysOnTop)
                    {
                        var words = new[] { "float", "window", "above" };
                        bool allPresent = words.All(w => text.Contains(w));
                        if (allPresent)
                        {
                            matchedAlwaysOnTop = true;
                            System.IO.File.AppendAllText(GetLogPath(), "[DEBUG] InterpretAsync matched catch-all: float/window/above\n");
                        }
                    }
                }
                if (matchedAlwaysOnTop)
                {
                    string? app = null;
                    var knownApps = new[] { "code", "msedge", "chrome", "firefox", "devenv", "opera", "brave" };
                    foreach (var candidate in knownApps)
                    {
                        if (text.Contains(candidate))
                        {
                            app = candidate;
                            break;
                        }
                    }
                    var action = new SetWindowAlwaysOnTopAction(app);
                    System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (always on top)\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Send key sequences
                if (text.StartsWith("press "))
                {
                    var keysText = text.Substring(6).Trim();
                    var action = new SendKeysAction(keysText);
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (send keys)\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Maximize/full screen window
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
                // Move window to other monitor (next)
                if ((text.Contains("move") || text.Contains("snap")) && text.Contains("window") && (text.Contains("other monitor") || text.Contains("next monitor") || text.Contains("other screen") || text.Contains("next screen") || text.Contains("my other monitor")))
                {
                    var action = new MoveWindowAction(
                        Target: "active",
                        Monitor: "next",
                        Position: "",
                        WidthPercent: 0,
                        HeightPercent: 0
                    );
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (next monitor)\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Move window to left half
                if ((text.Contains("move") || text.Contains("snap")) && text.Contains("window") && text.Contains("left"))
                {
                    var action = new MoveWindowAction(
                        Target: "active",
                        Monitor: "current",
                        Position: "left",
                        WidthPercent: 50,
                        HeightPercent: 100
                    );
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (left half)\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Open documents folder
                if (text.Contains("open documents"))
                {
                    var action = new OpenFolderAction("Documents");
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (documents)\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Fallback for unhandled commands: call AI
                string? currentApp = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
                string aiInput = text;
                if (!string.IsNullOrWhiteSpace(currentApp))
                {
                    aiInput += $"\nCurrentApplication: {currentApp}";
                }
                return InterpretWithAIAsync(aiInput);
            // End of InterpretAsync
            }
        // Supported apps for close tab
        private static readonly string[] SupportedCloseTabApps = new[] { "chrome", "msedge", "firefox", "brave", "opera", "code", "devenv" };

        public string ExecuteActionAsync(ActionBase action)
        {
            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] ExecuteActionAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
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
                // Move window to left half
                if (move.Position == "left" && move.WidthPercent == 50 && move.HeightPercent == 100)
                {
                    // Get monitor info
                        IntPtr monitor = MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                        MONITORINFOEX info = new MONITORINFOEX();
                        info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                        bool gotInfo = GetMonitorInfo(monitor, ref info);
                    if (!gotInfo)
                        return "Failed to get monitor info.";
                    var rect = info.rcWork;
                    int width = (rect.Right - rect.Left) / 2;
                    int height = rect.Bottom - rect.Top;
                    int x = rect.Left;
                    int y = rect.Top;
                    bool success = SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to move window left. Win32 error: {error}\n");
                        return $"Failed to move window left. Win32 error: {error}";
                    }
                    return "Window moved to left half.";
                }
                // Move window to right half
                if (move.Position == "right" && move.WidthPercent == 50 && move.HeightPercent == 100)
                {
                    // Get monitor info
                        IntPtr monitor = MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                        MONITORINFOEX info = new MONITORINFOEX();
                        info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                        bool gotInfo = GetMonitorInfo(monitor, ref info);
                    if (!gotInfo)
                        return "Failed to get monitor info.";
                    var rect = info.rcWork;
                    int width = (rect.Right - rect.Left) / 2;
                    int height = rect.Bottom - rect.Top;
                    int x = rect.Left + width;
                    int y = rect.Top;
                    bool success = SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to move window right. Win32 error: {error}\n");
                        return $"Failed to move window right. Win32 error: {error}";
                    }
                    return "Window moved to right half.";
                }
                // Move window to other monitor
                if (move.Position == "next" && move.WidthPercent == 0 && move.HeightPercent == 0)
                {
                    // Get active window handle
                    IntPtr activeHWnd = Commands.GetForegroundWindow();
                    if (activeHWnd == IntPtr.Zero)
                        return "No active window found.";
                    // Get current monitor
                    IntPtr currentMonitor = MonitorFromWindow(activeHWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                    MONITORINFOEX currentInfo = new MONITORINFOEX();
                    currentInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                    bool gotCurrentInfo = GetMonitorInfo(currentMonitor, ref currentInfo);
                    if (!gotCurrentInfo)
                        return "Failed to get current monitor info.";
                    // Find next monitor (circular)
                    IntPtr nextMonitor = IntPtr.Zero;
                    foreach (var monitor in GetAllMonitors())
                    {
                        if (monitor != currentMonitor)
                        {
                            nextMonitor = monitor;
                            break;
                        }
                    }
                    if (nextMonitor == IntPtr.Zero)
                        return "No other monitor found.";
                    // Get next monitor's working area
                    MONITORINFOEX nextInfo = new MONITORINFOEX();
                    nextInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                    bool gotNextInfo = GetMonitorInfo(nextMonitor, ref nextInfo);
                    if (!gotNextInfo)
                        return "Failed to get next monitor info.";
                    // Move window to the center of the next monitor
                    int width = (nextInfo.rcWork.Right - nextInfo.rcWork.Left);
                    int height = (nextInfo.rcWork.Bottom - nextInfo.rcWork.Top);
                    int widthPercent = (move.WidthPercent ?? 100);
                    int heightPercent = (move.HeightPercent ?? 100);
                    int x = nextInfo.rcWork.Left + (width - (width * widthPercent / 100)) / 2;
                    int y = nextInfo.rcWork.Top + (height - (height * heightPercent / 100)) / 2;
                    bool success = SetWindowPos(activeHWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to move window to next monitor. Win32 error: {error}\n");
                        return $"Failed to move window to next monitor. Win32 error: {error}";
                    }
                    return "Window moved to other monitor.";
                }
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
            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
            if (action == null)
            {
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }

        public string HandleNaturalAsync(string text, List<(string Command, string Description)> availableCommands)
        {
            var actionTask = InterpretAsync(text, availableCommands);
            actionTask.Wait();
            var action = actionTask.Result;
            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
            if (action == null)
            {
                // Suggest available commands if no match
                var suggestions = string.Join(", ", availableCommands.Select(c => c.Command));
                return $"[Natural mode] No matching action for: {text}. Available commands: {suggestions}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }

        public System.Threading.Tasks.Task<ActionBase?> InterpretAsync(string text, List<(string Command, string Description)> availableCommands)
        {
            text = (text ?? string.Empty).ToLowerInvariant().Trim();
            // Remove polite modifiers
            text = RemovePoliteModifiers(text);
            System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync input: {text}\n");

            // Fuzzy match against available commands
            var bestMatch = availableCommands
                .Select(cmd => (cmd.Command, Score: GetSimilarityScore(text, cmd.Command)))
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            // Threshold for fuzzy match (can be tuned)
            if (bestMatch.Score > 0.6)
            {
                // Map best match to action
                switch (bestMatch.Command)
                {
                    case "maximize window":
                        var action = new MoveWindowAction(Target: "active", Monitor: "current", Position: "center", WidthPercent: 100, HeightPercent: 100);
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name}\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                    case "move window to left half":
                        var leftAction = new MoveWindowAction(Target: "active", Monitor: "current", Position: "left", WidthPercent: 50, HeightPercent: 100);
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {leftAction.GetType().Name} (left half)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(leftAction);
                    case "move window to right half":
                        var rightAction = new MoveWindowAction(Target: "active", Monitor: "current", Position: "right", WidthPercent: 50, HeightPercent: 100);
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {rightAction.GetType().Name} (right half)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(rightAction);
                    case "move window to other monitor":
                        var nextAction = new MoveWindowAction(Target: "active", Monitor: "next", Position: "", WidthPercent: 0, HeightPercent: 0);
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {nextAction.GetType().Name} (next monitor)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(nextAction);
                    case "set window always on top":
                        var topAction = new SetWindowAlwaysOnTopAction(null);
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {topAction.GetType().Name} (always on top)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(topAction);
                    case "open downloads":
                        var downloadsAction = new OpenFolderAction("Downloads");
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {downloadsAction.GetType().Name} (downloads)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(downloadsAction);
                    case "open documents":
                        var documentsAction = new OpenFolderAction("Documents");
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {documentsAction.GetType().Name} (documents)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(documentsAction);
                    case "close tab":
                        var closeTabAction = new CloseTabAction();
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {closeTabAction.GetType().Name} (close tab)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(closeTabAction);
                    case "send keys":
                        var sendKeysAction = new SendKeysAction(text.Replace("press ", ""));
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {sendKeysAction.GetType().Name} (send keys)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(sendKeysAction);
                    case "launch app":
                        var launchAppAction = new LaunchAppAction(text.Replace("open ", ""));
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {launchAppAction.GetType().Name} (launch app)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(launchAppAction);
                    case "focus app":
                        // Not implemented, stub
                        break;
                    case "show help":
                        var helpAction = new ShowHelpAction();
                        System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {helpAction.GetType().Name} (help)\n");
                        return System.Threading.Tasks.Task.FromResult<ActionBase?>(helpAction);
                }
            }
            System.IO.File.AppendAllText("app.log", "[DEBUG] InterpretAsync: No match, returning null\n");
            return System.Threading.Tasks.Task.FromResult<ActionBase?>(null);
        }

        // Simple similarity score (normalized longest common subsequence)
        private static double GetSimilarityScore(string input, string command)
        {
            input = input.ToLowerInvariant();
            command = command.ToLowerInvariant();
            int lcs = LongestCommonSubsequence(input, command);
            return (double)lcs / Math.Max(input.Length, command.Length);
        }

        // Longest common subsequence algorithm
        private static int LongestCommonSubsequence(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    if (a[i - 1] == b[j - 1])
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    else
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
            return dp[a.Length, b.Length];
        }

        // Helper to enumerate all monitor handles
        private static IEnumerable<IntPtr> GetAllMonitors()
        {
            var monitors = new List<IntPtr>();
            bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
            {
                monitors.Add(hMonitor);
                return true;
            }
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, IntPtr.Zero);
            return monitors;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);
        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
    }
}
