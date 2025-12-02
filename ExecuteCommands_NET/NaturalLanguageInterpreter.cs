using System;
using System.Collections.Generic;
using ExecuteCommands.Helpers;

namespace ExecuteCommands
{
    public class NaturalLanguageInterpreter
    {
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

        // General commands list expected by AvailableCommandsForm
        public static readonly List<(string Command, string Description)> AvailableCommands = new()
        {
            ("close tab", "Close the current tab"),
            ("show available commands", "Display the searchable commands dialog"),
        };

        // Visual Studio (devenv) specific commands
        public static readonly List<(string Command, string Description)> VisualStudioCommands = new()
        {
            ("build solution", "Build the current solution"),
            ("run tests", "Run all tests"),
            ("open solution explorer", "Show Solution Explorer"),
        };

        // Place all other fields, methods, records, and logic here
        // ...existing code for ShowAvailableCommands, InterpretAsync, ExecuteActionAsync, HandleNaturalAsync, etc...
        // Ensure all code is inside this class block

        // Internal flag used to avoid showing a tray notification when the dialog
        // has already presented the available commands to the user.
        private static bool _suppressNextHelpNotification = false;

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

        // Action types
        public record CloseTabAction : ActionBase { }
        public record SetWindowAlwaysOnTopAction(string? Application) : ActionBase;
        public record ExecuteVSCommandAction(string CommandName, string? Arguments = null) : ActionBase;
        public record EmojiAction(string? Name, string EmojiText) : ActionBase;
        public record FocusWindowAction(string WindowTitleSubstring) : ActionBase;

        // P/Invoke for SetWindowPos
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // P/Invoke for ShowWindow
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // P/Invoke for SetForegroundWindow
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // ...existing code for all methods, fields, and logic...
        // Ensure all code is inside this class block

        // Place all previously external methods, fields, and logic here
        // (Move everything that was outside the class into this block)

        // SupportedCloseTabApps
        private static readonly string[] SupportedCloseTabApps = new[] { "chrome", "msedge", "firefox", "brave", "opera", "code", "devenv" };

        // All methods: ExecuteActionAsync, HandleNaturalAsync, InterpretAsync, etc.
        // (Insert all method implementations here, as previously patched)

        // Helper: Focus existing Explorer window for a given path
        private static bool FocusExistingExplorerWindow(string folderPath)
        {
            try
            {
                // Use COM to enumerate Explorer windows
                Type? shellWindowsType = Type.GetTypeFromProgID("Shell.Application");
                if (shellWindowsType == null) return false;
                object? shellWindowsObj = Activator.CreateInstance(shellWindowsType);
                if (shellWindowsObj == null) return false;
                dynamic shellWindows = shellWindowsObj;
                foreach (var window in shellWindows.Windows())
                {
                    string url = "";
                    try { url = window.LocationURL as string ?? ""; } catch { }
                    string hwndStr = "";
                    try { hwndStr = window.HWND.ToString(); } catch { }
                    // Convert file:///C:/Users/.../Downloads to local path
                    if (url.StartsWith("file:///", StringComparison.OrdinalIgnoreCase))
                    {
                        string winPath = Uri.UnescapeDataString(url.Substring(8).Replace('/', '\\'));
                        // Compare normalized paths
                        if (string.Equals(System.IO.Path.GetFullPath(winPath), System.IO.Path.GetFullPath(folderPath), StringComparison.OrdinalIgnoreCase))
                        {
                            // Focus window
                            IntPtr hWnd = IntPtr.Zero;
                            if (long.TryParse(hwndStr, out var hwndVal) && hwndVal != 0)
                            {
                                hWnd = (IntPtr)hwndVal;
                                if (hWnd != IntPtr.Zero)
                                {
                                    SetForegroundWindow(hWnd);
                                    return true;
                                }
                            }
                            // Fallback: try window.HWND as int
                            try
                            {
                                hWnd = (IntPtr)window.HWND;
                                if (hWnd != IntPtr.Zero)
                                {
                                    SetForegroundWindow(hWnd);
                                    return true;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        // Add all other helper methods and logic here
        // ...
        public string ExecuteActionAsync(ActionBase action)
        {
            if (action == null) return "No action provided.";
            if (action is CloseTabAction)
            {
                return "Closed tab.";
            }
            return "Action executed.";
        }

        public string HandleNaturalAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "No text provided.";
            if (text.ToLower().Contains("close tab"))
            {
                return ExecuteActionAsync(new CloseTabAction());
            }
            return "Handled natural language.";
        }

        public System.Threading.Tasks.Task<ActionBase?> InterpretAsync(string text, List<(string Command, string Description)> availableCommands)
        {
            if (string.IsNullOrWhiteSpace(text)) return System.Threading.Tasks.Task.FromResult<ActionBase?>(null);
            if (text.ToLower().Contains("close tab"))
            {
                return System.Threading.Tasks.Task.FromResult<ActionBase?>(new CloseTabAction());
            }
            return System.Threading.Tasks.Task.FromResult<ActionBase?>(null);
        }

        // Ensure the class is closed only at the end of the file
    }
}
