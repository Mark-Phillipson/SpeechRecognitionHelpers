using System;
namespace ExecuteCommands.Helpers
{
    // Handles window management actions (maximize, move, always on top, etc.)
    public class WindowManager
    {
        public static string ExecuteMoveWindow(MoveWindowAction move)
        {
            // Get active window handle
            IntPtr hWnd = ExecuteCommands.Commands.GetForegroundWindow();
            // Maximize logic
            if ((move.Position == "center" || move.Position == null) && move.WidthPercent == 100 && move.HeightPercent == 100 && move.Monitor != "next")
            {
                Win32ApiHelper.SetForegroundWindow(hWnd);
                int style = Win32ApiHelper.GetWindowLong(hWnd, Win32ApiHelper.GWL_STYLE);
                bool canMaximize = (style & Win32ApiHelper.WS_MAXIMIZEBOX) != 0;
                var className = new System.Text.StringBuilder(256);
                Win32ApiHelper.GetClassName(hWnd, className, className.Capacity);
                if (!canMaximize)
                {
                    return "Window cannot be maximized (missing maximize button).";
                }
                const int SW_MAXIMIZE = 3;
                bool success = Win32ApiHelper.ShowWindow(hWnd, SW_MAXIMIZE);
                if (!success)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    return $"Failed to maximize window. Win32 error: {error}";
                }
                return "Window maximized.";
            }
            // Move window to left half
            if (move.Position == "left" && move.WidthPercent == 50 && move.HeightPercent == 100)
            {
                IntPtr monitor = Win32ApiHelper.MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                Win32ApiHelper.MONITORINFOEX info = new Win32ApiHelper.MONITORINFOEX();
                info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32ApiHelper.MONITORINFOEX));
                bool gotInfo = monitor != IntPtr.Zero && Win32ApiHelper.GetMonitorInfo(monitor, ref info);
                if (!gotInfo)
                {
                    return "Failed to get monitor info.";
                }
                var rect = info.rcWork;
                int width = (rect.Right - rect.Left) / 2;
                int height = rect.Bottom - rect.Top;
                int x = rect.Left;
                int y = rect.Top;
                bool success = Win32ApiHelper.SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                if (!success)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    return $"Failed to move window left. Win32 error: {error}";
                }
                return "Window moved to left half.";
            }
            // Move window to right half
            if (move.Position == "right" && move.WidthPercent == 50 && move.HeightPercent == 100)
            {
                IntPtr monitor = Win32ApiHelper.MonitorFromWindow(hWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                Win32ApiHelper.MONITORINFOEX info = new Win32ApiHelper.MONITORINFOEX();
                info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32ApiHelper.MONITORINFOEX));
                bool gotInfo = monitor != IntPtr.Zero && Win32ApiHelper.GetMonitorInfo(monitor, ref info);
                if (!gotInfo)
                {
                    return "Failed to get monitor info.";
                }
                var rect = info.rcWork;
                int width = (rect.Right - rect.Left) / 2;
                int height = rect.Bottom - rect.Top;
                int x = rect.Left + width;
                int y = rect.Top;
                bool success = Win32ApiHelper.SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                if (!success)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    return $"Failed to move window right. Win32 error: {error}";
                }
                return "Window moved to right half.";
            }
            // Move window to other monitor
            if (move.Monitor == "next" && (move.WidthPercent == 0 || move.WidthPercent == null) && (move.HeightPercent == 0 || move.HeightPercent == null))
            {
                IntPtr activeHWnd = ExecuteCommands.Commands.GetForegroundWindow();
                if (activeHWnd == IntPtr.Zero)
                {
                    return "No active window found.";
                }
                IntPtr currentMonitor = Win32ApiHelper.MonitorFromWindow(activeHWnd, 2 /*MONITOR_DEFAULTTONEAREST*/);
                Win32ApiHelper.MONITORINFOEX currentInfo = new Win32ApiHelper.MONITORINFOEX();
                currentInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32ApiHelper.MONITORINFOEX));
                bool gotCurrentInfo = Win32ApiHelper.GetMonitorInfo(currentMonitor, ref currentInfo);
                if (!gotCurrentInfo)
                {
                    return "Failed to get current monitor info.";
                }
                IntPtr nextMonitor = IntPtr.Zero;
                foreach (IntPtr monitor in Win32ApiHelper.GetAllMonitors())
                {
                    if (monitor != currentMonitor)
                    {
                        nextMonitor = monitor;
                        break;
                    }
                }
                if (nextMonitor == IntPtr.Zero)
                {
                    return "No other monitor found.";
                }
                Win32ApiHelper.MONITORINFOEX nextInfo = new Win32ApiHelper.MONITORINFOEX();
                nextInfo.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32ApiHelper.MONITORINFOEX));
                bool gotNextInfo = Win32ApiHelper.GetMonitorInfo(nextMonitor, ref nextInfo);
                if (!gotNextInfo)
                {
                    return "Failed to get next monitor info.";
                }
                int width = (nextInfo.rcWork.Right - nextInfo.rcWork.Left);
                int height = (nextInfo.rcWork.Bottom - nextInfo.rcWork.Top);
                int widthPercent = move.WidthPercent.HasValue ? move.WidthPercent.Value : 100;
                int heightPercent = move.HeightPercent.HasValue ? move.HeightPercent.Value : 100;
                int x = nextInfo.rcWork.Left + (width - (width * widthPercent / 100)) / 2;
                int y = nextInfo.rcWork.Top + (height - (height * heightPercent / 100)) / 2;
                bool success = Win32ApiHelper.SetWindowPos(activeHWnd, IntPtr.Zero, x, y, width, height, 0x0040 /*SWP_SHOWWINDOW*/);
                if (!success)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    return $"Failed to move window to next monitor. Win32 error: {error}";
                }
                return "Window moved to other monitor.";
            }
            return "[Stub] Window move not implemented for: " + move.ToString();
        }
    }
}
