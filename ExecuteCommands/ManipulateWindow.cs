using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsInput;

namespace ExecuteCommands
{
     public  class ManipulateWindow
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        public void BringMainWindowToFront(string processName)
        {
            // get the process
            Process bProcess = Process.GetProcessesByName(processName).FirstOrDefault();
            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.APPS);
            // check if the process is running
            if (bProcess != null)
            {
                // check if the window is hidden / minimized
                if (bProcess.MainWindowHandle == IntPtr.Zero)
                {
                    // the window is hidden so try to restore it before setting focus.
                    ShowWindow(bProcess.Handle, ShowWindowEnum.Restore);
                }

                // set user the focus to the window
                SetForegroundWindow(bProcess.MainWindowHandle);
            }
            else
            {
                // the process is not running, so start it
                Process.Start(processName);
            }
            inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.APPS);
        }
    }
}
