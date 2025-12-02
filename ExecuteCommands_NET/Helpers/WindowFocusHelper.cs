using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExecuteCommands.Helpers
{
    public static class WindowFocusHelper
    {
        private const string WindowsTerminalClass = "CASCADIA_HOSTING_WINDOW_CLASS";

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
