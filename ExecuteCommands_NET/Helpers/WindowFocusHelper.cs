
    using System;
    using System.Runtime.InteropServices;
    using System.Diagnostics;

namespace ExecuteCommands.Helpers
{
    public static class WindowFocusHelper
    {
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        private const string WindowsTerminalClass = "CASCADIA_HOSTING_WINDOW_CLASS";
        private const string ZoomWindowClass = "zoom_acc_notify_wnd"; // Actual Zoom main window class from user system
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        private const int SW_RESTORE = 9;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        public static bool FocusWindowByTitle(string titleSubstring)
        {
            IntPtr foundHwnd = IntPtr.Zero;
            string foundProcName = "";
            string foundClassName = "";
            string foundWindowTitle = "";

            // Helper to generate all singular/plural combinations for each word
            static IEnumerable<string> GetTitleVariants(string input)
            {
                var words = input.Split(' ');
                List<List<string>> wordVariants = new List<List<string>>();
                foreach (var word in words)
                {
                    var variants = new List<string> { word };
                    if (word.EndsWith("s") && word.Length > 1)
                    {
                        // Remove trailing 's' for singular
                        variants.Add(word.Substring(0, word.Length - 1));
                    }
                    else if (word.Length > 1)
                    {
                        // Add plural form
                        variants.Add(word + "s");
                    }
                    wordVariants.Add(variants);
                }
                // Generate all combinations
                var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                void Combine(int idx, List<string> current)
                {
                    if (idx == wordVariants.Count)
                    {
                        results.Add(string.Join(" ", current));
                        return;
                    }
                    foreach (var variant in wordVariants[idx])
                    {
                        current.Add(variant);
                        Combine(idx + 1, current);
                        current.RemoveAt(current.Count - 1);
                    }
                }
                Combine(0, new List<string>());
                return results;
            }

            var titleVariants = GetTitleVariants(titleSubstring);
            string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");
            logPath = System.IO.Path.GetFullPath(logPath);
            System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: Checking variants: {string.Join(", ", titleVariants)}\n");

            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd)) return true;
                var title = new System.Text.StringBuilder(256);
                GetWindowText(hWnd, title, title.Capacity);
                string windowTitle = title.ToString();
                System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: Window title found: '{windowTitle}'\n");
                foreach (var variant in titleVariants)
                {
                    var variantWords = variant.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    bool allWordsPresent = true;
                    foreach (var word in variantWords)
                    {
                        if (!windowTitle.Contains(word, StringComparison.OrdinalIgnoreCase))
                        {
                            allWordsPresent = false;
                            break;
                        }
                    }
                    if (allWordsPresent)
                    {
                        System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: Fuzzy matched variant '{variant}' in window title '{windowTitle}'\n");
                        foundHwnd = hWnd;
                        foundWindowTitle = windowTitle;
                        uint pid;
                        GetWindowThreadProcessId(hWnd, out pid);
                        try
                        {
                            var proc = Process.GetProcessById((int)pid);
                            foundProcName = proc.ProcessName;
                            var className = new System.Text.StringBuilder(256);
                            GetClassName(hWnd, className, className.Capacity);
                            foundClassName = className.ToString();
                        }
                        catch { }
                        return false; // Stop enumeration
                    }
                }
                return true;
            }, IntPtr.Zero);
            if (foundHwnd != IntPtr.Zero)
            {
                bool restoreResult = ShowWindow(foundHwnd, SW_RESTORE);
                if (!restoreResult)
                {
                    int err = Marshal.GetLastWin32Error();
                    System.IO.File.AppendAllText(logPath, $"[ERROR] ShowWindow failed. GetLastError={err}\n");
                }
                IntPtr foregroundHwnd = GetForegroundWindow();
                uint foregroundThread = GetWindowThreadProcessId(foregroundHwnd, out _);
                uint targetThread = GetWindowThreadProcessId(foundHwnd, out _);
                bool attachResult = AttachThreadInput(foregroundThread, targetThread, true);
                if (!attachResult)
                {
                    int err = Marshal.GetLastWin32Error();
                    System.IO.File.AppendAllText(logPath, $"[ERROR] AttachThreadInput failed. GetLastError={err}\n");
                }
                // Send dummy Alt key input to help bypass focus restrictions
                //keybd_event(0x12, 0, 0, 0); // Alt down
                //keybd_event(0x12, 0, 2, 0); // Alt up
                bool focusResult = SetForegroundWindow(foundHwnd);
                if (!focusResult)
                {
                    int err = Marshal.GetLastWin32Error();
                    System.IO.File.AppendAllText(logPath, $"[ERROR] SetForegroundWindow failed. GetLastError={err}\n");
                }
                // Check if foreground window actually changed
                IntPtr afterFocusHwnd = GetForegroundWindow();
                bool actuallyFocused = (afterFocusHwnd == foundHwnd);
                System.IO.File.AppendAllText(logPath, $"[DEBUG] After SetForegroundWindow: foregroundHwnd={afterFocusHwnd}, expectedHwnd={foundHwnd}, actuallyFocused={actuallyFocused}\n");
                if (attachResult)
                    AttachThreadInput(foregroundThread, targetThread, false); // detach
                System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: hwnd={foundHwnd}, proc={foundProcName}, class={foundClassName}, title={foundWindowTitle}, ShowWindow(SW_RESTORE)={restoreResult}, AttachThreadInput={attachResult}, SetForegroundWindow={focusResult}\n");
                // Fallback: simulate mouse click if not focused (either SetForegroundWindow failed or window not actually foreground)
                if (!focusResult || !actuallyFocused)
                {
                    System.IO.File.AppendAllText(logPath, "[DEBUG] FocusWindowByTitle: SetForegroundWindow failed or window not foreground, trying mouse click fallback.\n");
                    RECT rect;
                    if (GetWindowRect(foundHwnd, out rect))
                    {
                        int x = rect.Left + 10;
                        int y = rect.Top + 10;
                        SetCursorPos(x, y);
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                        System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: Simulated mouse click at ({x},{y}) on window.\n");
                        focusResult = SetForegroundWindow(foundHwnd);
                        if (!focusResult)
                        {
                            int err = Marshal.GetLastWin32Error();
                            System.IO.File.AppendAllText(logPath, $"[ERROR] SetForegroundWindow after mouse click failed. GetLastError={err}\n");
                        }
                        afterFocusHwnd = GetForegroundWindow();
                        actuallyFocused = (afterFocusHwnd == foundHwnd);
                        System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: SetForegroundWindow after mouse click: {focusResult}, actuallyFocused: {actuallyFocused}\n");
                    }
                }
                // Final fallback: show three focus apps (Ctrl+Alt+Tab) if still not focused
                if (!focusResult || !actuallyFocused)
                {
                    System.IO.File.AppendAllText(logPath, "[DEBUG] FocusWindowByTitle: All focus attempts failed, sending Ctrl+Alt+Tab as last resort.\n");
                    // Send Ctrl+Alt+Tab key sequence
                    keybd_event(0x11, 0, 0, 0); // Ctrl down
                    keybd_event(0x12, 0, 0, 0); // Alt down
                    keybd_event(0x09, 0, 0, 0); // Tab down
                    keybd_event(0x09, 0, 2, 0); // Tab up
                    keybd_event(0x12, 0, 2, 0); // Alt up
                    keybd_event(0x11, 0, 2, 0); // Ctrl up
                    System.IO.File.AppendAllText(logPath, "[DEBUG] FocusWindowByTitle: Sent Ctrl+Alt+Tab.\n");
                }
                return actuallyFocused;
            }
            else
            {
                System.IO.File.AppendAllText(logPath, $"[DEBUG] FocusWindowByTitle: No matching window found. Last proc={foundProcName}, class={foundClassName}\n");
            }
            return false;
        }

        public static bool FocusZoom()
        {
            // For backward compatibility, call the generic method
            return FocusWindowByTitle("zoom");
        }

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static bool FocusWindowsTerminal()
        {
            IntPtr foundHwnd = IntPtr.Zero;
            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd)) return true;
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);
                try
                {
                    var proc = Process.GetProcessById((int)pid);
                    if (proc.ProcessName.Equals("wt", StringComparison.OrdinalIgnoreCase))
                    {
                        var className = new System.Text.StringBuilder(256);
                        GetClassName(hWnd, className, className.Capacity);
                        if (className.ToString().Equals(WindowsTerminalClass, StringComparison.OrdinalIgnoreCase))
                        {
                            foundHwnd = hWnd;
                            return false; // Stop enumeration
                        }
                    }
                }
                catch { }
                return true;
            }, IntPtr.Zero);
            if (foundHwnd != IntPtr.Zero)
            {
                return SetForegroundWindow(foundHwnd);
            }
            return false;
        }
    }
}
