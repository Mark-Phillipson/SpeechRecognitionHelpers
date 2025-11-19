using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExecuteCommands
{
    public static class CurrentApplicationHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static string GetCurrentProcessName()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return null;
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            try
            {
                Process proc = Process.GetProcessById(pid);
                return proc.ProcessName.ToLower();
            }
            catch
            {
                return null;
            }
        }
    }
}
