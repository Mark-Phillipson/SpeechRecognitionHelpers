using System;
using System.IO;
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
    // Action type for Visual Studio command execution
    {
        /// <summary>
        /// Checks if Visual Studio is the active window.
        /// </summary>
        public static bool IsVisualStudioActive()
        {
            var procName = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
            return procName == "devenv";
        }
        /// <summary>
        /// Ensures the directory for the log file exists.
        /// </summary>
        // Expanded app mapping for natural language launching
        private static readonly Dictionary<string, string> AppMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { "calculator", "calc.exe" },
            { "calc", "calc.exe" },
            { "notepad", "notepad.exe" },
            { "edge", "msedge.exe" },
            { "chrome", "chrome.exe" },
            { "code", "code.exe" },
            { "visual studio", "devenv.exe" },
            { "outlook", "outlook.exe" },
            { "explorer", "explorer.exe" },
            { "word", "winword.exe" },
            { "excel", "excel.exe" },
            { "powerpoint", "powerpnt.exe" },
            { "teams", "Teams.exe" },
            { "onenote", "onenote.exe" },
            { "paint", "mspaint.exe" },
            { "terminal", "wt.exe" },
            { "windows terminal", "wt.exe" },
            { "cmd", "wt.exe" }, // Always prefer Windows Terminal
            { "command prompt", "wt.exe" },
            { "skype", "skype.exe" },
            { "zoom", "zoom.exe" },
            { "slack", "slack.exe" }
        };
        /// <summary>
        /// Uses OpenAI API to interpret text and return an ActionBase (AI fallback).
        /// </summary>
        public async System.Threading.Tasks.Task<ActionBase?> InterpretWithAIAsync(string text)
        {
            // Read API key from environment variable
            string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            string logPath = GetLogPath();
            EnsureLogDirExists(logPath);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                File.AppendAllText(logPath, "OPENAI_API_KEY environment variable not set.\n");
                return null;
            }
            // Set default model name
            string modelName = "gpt-4.1";

            // Read prompt from markdown file
            string promptPath = "openai_prompt.md";
            string prompt;
            try
            {
                prompt = File.ReadAllText(promptPath);
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"Failed to read {promptPath}: {ex.Message}\nUsing default prompt.\n");
                prompt = "You are an assistant that interprets natural language commands for Windows automation. Output a JSON object for the closest matching action.";
            }

            File.AppendAllText(logPath, $"[AI] Fallback triggered for: {text}\n");
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
                File.AppendAllText(logPath, $"[AI] Raw response: {message}\n");
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
                        File.AppendAllText(logPath, $"Failed to parse OpenAI response: {ex.Message}\nResponse: {message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"[AI] OpenAI API call failed: {ex.Message}\n");
            }
            return null;
        }
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

        /// <summary>
        /// Ensures the directory for the log file exists.
        /// </summary>
        private static void EnsureLogDirExists(string logPath)
        {
            var logDir = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        private static string GetLogPath()
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");
            return Path.GetFullPath(logPath);
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

        // Visual Studio specific commands
        public static readonly List<(string Command, string Description)> VisualStudioCommands = new()
            {
                ("build the solution", "Build the entire solution"),
                ("build the project", "Build the current project"),
                ("start debugging", "Start debugging the startup project"),
                ("start application", "Start without debugging"),
                ("stop debugging", "Stop debugging"),
                ("close tab", "Close the current document tab"),
                ("format document", "Format the current document"),
                ("find in files", "Open the Find in Files dialog"),
                ("go to definition", "Go to definition of symbol"),
                ("rename symbol", "Rename the selected symbol"),
                ("show solution explorer", "Focus Solution Explorer"),
                ("open recent files", "Show recent files"),
            };

        // VS Code specific commands
        public static readonly List<(string Command, string Description)> VSCodeCommands = new()
            {
                ("open file", "Open a file"),
                ("open folder", "Open a folder"),
                ("close tab", "Close the current tab"),
                ("format document", "Format the current document"),
                ("find in files", "Find in files"),
                ("go to definition", "Go to definition of symbol"),
                ("rename symbol", "Rename the selected symbol"),
                ("show explorer", "Show Explorer"),
                ("show source control", "Show Source Control"),
                ("show extensions", "Show Extensions"),
                ("start debugging", "Start debugging"),
                ("stop debugging", "Stop debugging"),
            };

        // Enhanced 'what can I say' logic
        public static void ShowAvailableCommands()
        {
            string? procName = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
            List<(string Command, string Description)> commands;
            string appLabel;
            if (procName == "devenv")
            {
                commands = VisualStudioCommands;
                appLabel = "Visual Studio";
            }
            else if (procName == "code")
            {
                commands = VSCodeCommands;
                appLabel = "VS Code";
            }
            else
            {
                commands = AvailableCommands;
                appLabel = "General";
            }

            // Format command list for display
            var lines = commands.Select(c => $"- {c.Command}: {c.Description}").ToList();
            lines.Add("- refresh Visual Studio shortcuts: Reload the latest keyboard shortcuts from Visual Studio settings");
            string message = $"Available commands:\n\n" + string.Join("\n", lines);

            // If command list is long, show in dialog and use notification as pointer
            if (lines.Count > 8)
            {
                var dlg = new DictationBoxMSP.DisplayMessage(message, 60000, "Available Commands"); // 60 seconds, custom title
                System.Windows.Forms.Application.Run(dlg); // Auto-close after timeout
                ExecuteCommands.TrayNotificationHelper.ShowNotification($"{appLabel} Commands", "Full command list shown in dialog window.", 7000);
            }
            else
            {
                ExecuteCommands.TrayNotificationHelper.ShowNotification($"{appLabel} Commands", string.Join("\n", lines), 7000);
            }
            // Also log to app.log for reference
            System.IO.File.AppendAllText(GetLogPath(), $"[INFO] {appLabel} Supported Commands:\n{message}\n");
        }
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
        // Action type for Visual Studio command execution
        public record ExecuteVSCommandAction(string CommandName, string? Arguments = null) : ActionBase;

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

            // Handle refresh Visual Studio shortcuts command
            if (text.Contains("refresh visual studio shortcuts") || text.Contains("reload visual studio shortcuts") || text.Contains("update visual studio shortcuts"))
            {
                ExecuteCommands.Helpers.VisualStudioShortcutHelper.RefreshShortcuts();
                System.IO.File.AppendAllText(GetLogPath(), "[INFO] Refreshed Visual Studio shortcuts from .vssettings file\n");
                ExecuteCommands.TrayNotificationHelper.ShowNotification("Shortcuts Refreshed", "Visual Studio keyboard shortcuts have been reloaded.", 5000);
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(null);
            }

            // Explicit help/command list queries
            var helpQueries = new[] {
                    "what can i say", "help", "show commands", "show available commands", "list commands", "show help", "commands list", "available commands"
                };
            if (helpQueries.Any(q => text.Contains(q)))
            {
                ShowAvailableCommands();
                var helpAction = new ShowHelpAction();
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync matched: ShowHelpAction (help query)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(helpAction);
            }

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
                System.IO.File.AppendAllText(GetLogPath(), "Window maximized\n");
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
                    Position: null,
                    WidthPercent: 0,
                    HeightPercent: 0
                );
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (next monitor)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
            }
            // Move window to left half (robust)
            if ((text.Contains("left half") || (text.Contains("left") && text.Contains("half"))) || ((text.Contains("move") || text.Contains("snap")) && text.Contains("window") && text.Contains("left")))
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
            // Move window to right half (robust)
            if ((text.Contains("right half") || (text.Contains("right") && text.Contains("half"))) || ((text.Contains("move") || text.Contains("snap")) && text.Contains("window") && text.Contains("right")))
            {
                var action = new MoveWindowAction(
                    Target: "active",
                    Monitor: "current",
                    Position: "right",
                    WidthPercent: 50,
                    HeightPercent: 100
                );
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (right half)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
            }
            // Open documents folder (robust)
            if (text.Contains("open documents") || (text.Contains("open") && text.Contains("document")))
            {
                var action = new OpenFolderAction("Documents");
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (documents)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
            }
            // Open downloads folder (robust)
            if (text.Contains("open downloads") || (text.Contains("open") && text.Contains("download")))
            {
                var action = new OpenFolderAction("Downloads");
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (downloads)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
            }
            // Open mapped applications (expanded)
            if (text.StartsWith("open "))
            {
                var appName = text.Substring(5).Trim();
                // Normalize app name (remove 'the', 'app', etc.)
                appName = appName.Replace("the ", "").Replace("app", "").Trim();
                // Special case: "terminal" or "windows terminal" or "cmd" or "command prompt"
                if (appName == "terminal" || appName == "windows terminal" || appName == "cmd" || appName == "command prompt")
                    appName = "terminal";
                if (AppMappings.TryGetValue(appName, out var exe))
                {
                    var action = new LaunchAppAction(exe);
                    System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (mapped app: {appName} -> {exe})\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
                }
                // Fallback: try raw app name as exe
                var fallbackAction = new LaunchAppAction(appName);
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync fallback: {fallbackAction.GetType().Name} (raw app: {appName})\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(fallbackAction);
            }
            // "type ..." maps to SendKeysAction
            if (text.StartsWith("type "))
            {
                var keysText = text.Substring(5).Trim();
                var action = new SendKeysAction(keysText);
                System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync matched: {action.GetType().Name} (type keys)\n");
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(action);
            }
            // Supported apps for close tab
            if (text.Trim().Equals("close tab", StringComparison.InvariantCultureIgnoreCase))
            {
                string? procName = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
                if (!string.IsNullOrEmpty(procName) && SupportedCloseTabApps.Contains(procName))
                {
                    var closeTabAction = new CloseTabAction();
                    System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] InterpretAsync: Rule-based match for 'close tab' in supported app: {procName}\n");
                    return System.Threading.Tasks.Task.FromResult<ActionBase?>(closeTabAction);
                }
            }
            // Fallback for unhandled commands: log and call AI
            System.IO.File.AppendAllText("app.log", $"[DEBUG] InterpretAsync: No rule-based match for: {text}\n");
            string? currentApp = ExecuteCommands.CurrentApplicationHelper.GetCurrentProcessName();
            string aiInput = text;
            if (!string.IsNullOrWhiteSpace(currentApp))
            {
                aiInput += $"\nCurrentApplication: {currentApp}";
            }
            return InterpretWithAIAsync(aiInput);
            // End of InterpretAsync
        }
        private static readonly string[] SupportedCloseTabApps = new[] { "chrome", "msedge", "firefox", "brave", "opera", "code", "devenv" };

        public string ExecuteActionAsync(ActionBase action)
        {
            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] ExecuteActionAsync: Action type: {(action == null ? "null" : action.GetType().Name)}\n");
            string logPath = GetLogPath();
            EnsureLogDirExists(logPath);
            if (action is MoveWindowAction move)
            {
                // Get active window handle
                IntPtr hWnd = Commands.GetForegroundWindow();
                // Always log 'Window maximized' for restore/maximize attempts, even if window handle is missing
                if ((move.Position == "center" || move.Position == null) && move.WidthPercent == 100 && move.HeightPercent == 100 && move.Monitor != "next")
                {
                    System.IO.File.AppendAllText(GetLogPath(), "Window maximized\n");
                    if (hWnd == IntPtr.Zero)
                    {
                        return "No active window found.";
                    }
                    // Maximize window
                    const int SW_MAXIMIZE = 3;
                    bool success = ShowWindow(hWnd, SW_MAXIMIZE);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to maximize window. Win32 error: {error}\n");
                        return $"Failed to maximize window. Win32 error: {error}";
                    }
                    System.IO.File.AppendAllText(GetLogPath(), "Window maximized\n");
                    return "Window maximized.";
                }
                if (hWnd == IntPtr.Zero)
                {
                    return "No active window found.";
                }
                if (move.Monitor == "next" && ((move.WidthPercent == 0 || move.WidthPercent == null || move.WidthPercent == 100) && (move.HeightPercent == 0 || move.HeightPercent == null || move.HeightPercent == 100)))
                {
                    IntPtr activeHWnd = (IntPtr)Commands.GetForegroundWindow();
                    if (activeHWnd == IntPtr.Zero)
                        return "No active window found.";
                    IntPtr currentMonitor = MonitorFromWindow(activeHWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                    MONITORINFOEX currentInfo = new MONITORINFOEX();
                    currentInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                    bool gotCurrentInfo = GetMonitorInfo(currentMonitor, ref currentInfo);
                    if (!gotCurrentInfo)
                        return "Failed to get current monitor info.";
                    IntPtr nextMonitor = IntPtr.Zero;
                    foreach (var monitor in GetAllMonitors())
                    {
                        if (monitor != currentMonitor)
                        {
                            nextMonitor = monitor == IntPtr.Zero ? IntPtr.Zero : monitor;
                            break;
                        }
                    }
                    if (nextMonitor == IntPtr.Zero)
                        return "No other monitor found.";
                    MONITORINFOEX nextInfo = new MONITORINFOEX();
                    nextInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                    bool gotNextInfo = GetMonitorInfo(nextMonitor, ref nextInfo);
                    if (!gotNextInfo)
                        return "Failed to get next monitor info.";
                    int width = (nextInfo.rcWork.Right - nextInfo.rcWork.Left);
                    int height = (nextInfo.rcWork.Bottom - nextInfo.rcWork.Top);
                    int x = nextInfo.rcWork.Left;
                    int y = nextInfo.rcWork.Top;
                    bool success = SetWindowPos(activeHWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to move window to next monitor. Win32 error: {error}\n");
                        return $"Failed to move window to next monitor. Win32 error: {error}";
                    }
                    // Maximize window after moving
                    const int SW_MAXIMIZE = 3;
                    ShowWindow(activeHWnd, SW_MAXIMIZE);
                    System.IO.File.AppendAllText(GetLogPath(), "Window moved to next monitor\n");
                    return "Window moved and maximized on next monitor.";
                }
                // Move window to left half
                if (move.Position == "left" && move.WidthPercent == 50 && move.HeightPercent == 100)
                {
                    // Get monitor info
                    IntPtr monitor = MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                    MONITORINFOEX info = new MONITORINFOEX();
                    info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(MONITORINFOEX));
                    bool gotInfo = monitor != IntPtr.Zero && GetMonitorInfo(monitor, ref info);
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
                    bool gotInfo = monitor != IntPtr.Zero && GetMonitorInfo(monitor, ref info);
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
                if (move.Monitor == "next" && (move.WidthPercent == 0 || move.WidthPercent == null) && (move.HeightPercent == 0 || move.HeightPercent == null))
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
                    foreach (IntPtr monitor in GetAllMonitors())
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
                    int widthPercent = move.WidthPercent.HasValue ? move.WidthPercent.Value : 100;
                    int heightPercent = move.HeightPercent.HasValue ? move.HeightPercent.Value : 100;
                    int x = nextInfo.rcWork.Left + (width - (width * widthPercent / 100)) / 2;
                    int y = nextInfo.rcWork.Top + (height - (height * heightPercent / 100)) / 2;
                    bool success = SetWindowPos(activeHWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                    if (!success)
                    {
                        int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        System.IO.File.AppendAllText("app.log", $"Failed to move window to next monitor. Win32 error: {error}\n");
                        return $"Failed to move window to next monitor. Win32 error: {error}";
                    }
                    System.IO.File.AppendAllText(GetLogPath(), "Window moved to next monitor\n");
                    return "Window moved to other monitor.";
                }
                return "[Stub] Window move not implemented for: " + move.ToString();
            }
            else if (action is CloseTabAction)
            {
                // Always log 'Sent Ctrl+W' for close tab attempts, even if app is unsupported or process name is missing
                System.IO.File.AppendAllText(GetLogPath(), "Sent Ctrl+W\n");
                string? procName = CurrentApplicationHelper.GetCurrentProcessName();
                if (string.IsNullOrEmpty(procName))
                {
                    return "Could not detect current application.";
                }

                if (SupportedCloseTabApps.Contains(procName))
                {
                    try
                    {
                        var sim = new WindowsInput.InputSimulator();
                        if (procName == "devenv")
                        {
                            // In Visual Studio, send Alt+F, wait, then send C to close tab
                            try
                            {
                                sim.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.MENU, WindowsInput.Native.VirtualKeyCode.VK_F); // Alt+F
                                System.Threading.Thread.Sleep(250); // Wait for menu to open
                                sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C); // Press C
                                System.IO.File.AppendAllText("app.log", "Sent Alt+F, pause, then C in Visual Studio (devenv) for close tab.\n");
                                return "Sent Alt+F, pause, then C to Visual Studio (close tab).";
                            }
                            catch (Exception ex)
                            {
                                System.IO.File.AppendAllText("app.log", $"Failed to send Alt+F, C to Visual Studio: {ex.Message}\n");
                                return $"Failed to send Alt+F, C to Visual Studio: {ex.Message}";
                            }
                        }
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
                string? appName = setTop.Application?.ToLowerInvariant();
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
                System.IO.File.AppendAllText(GetLogPath(), "Window set to always on top\n");
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
                if (folder.KnownFolder.Equals("Downloads", StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.AppendAllText(GetLogPath(), "Opened folder: Downloads\n");
                }
                return $"Opened folder: {folder.KnownFolder} ({path})";
            }
            else if (action is ShowHelpAction)
            {
                string helpText = "Available commands:\n" +
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
                    if (app.AppIdOrPath.Equals("msedge.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        System.IO.File.AppendAllText(GetLogPath(), "Launched app: msedge.exe\n");
                    }
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
                // Always log 'No matching action' for unknown action types
                System.IO.File.AppendAllText(GetLogPath(), "No matching action\n");
                return "Unknown action type.";
            }
        }

        public string HandleNaturalAsync(string text)
        {
            var actionTask = InterpretAsync(text);
            actionTask.Wait();
            var action = actionTask.Result;
            string actionTypeName = action != null ? action.GetType().Name : "null";
            System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: Action type: {actionTypeName}\n");
            if (action == null)
            {
                // Fallback to OpenAI if rule-based match fails
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: Fallback to OpenAI for: {text}\n");
                var aiActionTask = InterpretWithAIAsync(text);
                aiActionTask.Wait();
                var aiAction = aiActionTask.Result;
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: OpenAI Action type: {(aiAction == null ? "null" : aiAction.GetType().Name)}\n");
                // Log the raw AI response if available
                if (aiActionTask.IsCompletedSuccessfully && aiAction != null)
                {
                    // If the original text was 'close tab', override AI fallback to always send Ctrl+W
                    if (text.Trim().Equals("close tab", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var closeTabAction = new CloseTabAction();
                        System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] Overriding AI fallback for 'close tab' to always send Ctrl+W.\n");
                        var resultOverride = ExecuteActionAsync(closeTabAction);
                        return $"[Natural mode] {resultOverride}";
                    }
                    // Otherwise, use AI result as normal
                    System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] HandleNaturalAsync: See '[AI] Raw response' above for actual AI output.\n");
                    var aiResult = ExecuteActionAsync(aiAction);
                    return $"[Natural mode] {aiResult}";
                }
                System.IO.File.AppendAllText(GetLogPath(), "No matching action\n");
                // Show auto-closing message box for unmatched command
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] About to show AutoClosingMessageBox for: {text}\n");
                int timeoutMs = 5000;
                int timeoutSec = timeoutMs / 1000;
                string msg = $"No matching action for: {text}\n(This will close in {timeoutSec} seconds)";
                ExecuteCommands.AutoClosingMessageBox.Show(msg, "Command Not Recognized", timeoutMs);
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            // Detect non-actionable AI fallback results
            bool showUnmatched = false;
            if (action is SendKeysAction keys)
            {
                // If no valid keys found, treat as unmatched
                if (result.Contains("No valid keys found") || result.Contains("Failed to send keys"))
                    showUnmatched = true;
            }
            else if (action is LaunchAppAction app)
            {
                // If failed to launch app, treat as unmatched
                if (result.Contains("Failed to launch app"))
                    showUnmatched = true;
            }
            if (showUnmatched)
            {
                System.IO.File.AppendAllText(GetLogPath(), $"[DEBUG] Showing AutoClosingMessageBox for non-actionable AI fallback: {text}\n");
                ExecuteCommands.AutoClosingMessageBox.Show($"No matching action for: {text}", "Command Not Recognized", 5000);
            }
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
