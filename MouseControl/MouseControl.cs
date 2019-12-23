using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace MouseControl
{
    public class MouseControl
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern void mouse_event
    (int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public struct POINT
        {
            public int X;
            public int Y;
        }
        public enum MouseEventType : int
        {
            LeftDown = 0x02,
            LeftUp = 0x04,
            RightDown = 0x08,
            RightUp = 0x10
        }
        private string[] _arguments;
        int millisecondsDelay;
        public MouseControl(string[] arguments)
        {
            _arguments = arguments;
        }
        public void PerformControl()
        {
            GetDelayFromSettings();
            Process[] processes = KillCurrentMouseControlProcess();
            POINT point;
            GetCursorPos(out point);


            //foreach (var argument in arguments)
            //{
            //    MessageBox.Show(argument);
            //}


            int delay = 0;
            if (Int32.TryParse(_arguments[1].Substring(1), out delay))
            {
                Properties.Settings.Default.Delay = delay;
                Properties.Settings.Default.Save();
                millisecondsDelay = Properties.Settings.Default.Delay;
                //MessageBox.Show(arguments[1] + " delay" + millisecondsDelay);
                var lastCommand = Properties.Settings.Default.LastCommand;
                _arguments[1] = lastCommand;
            }
            else
            {
                Properties.Settings.Default.LastCommand = _arguments[1];
                Properties.Settings.Default.Save();
            }
            var step = 1;
            if (millisecondsDelay == 1)
            {
                step = 50;
                millisecondsDelay = 300;
            }

            if (_arguments[1].ToLower().Contains("/right-click"))
            {
                mouse_event((int)MouseEventType.RightDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.RightUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (_arguments[1].ToLower().Contains("/upper-left"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY > 0; counterY = counterY - step)
                {
                    counterX--;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/upper-right"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY > 0; counterY = counterY - step)
                {
                    counterX++;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/lower-left"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY < 1200; counterY = counterY + step)
                {
                    counterX--;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/lower-right"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY < 1200; counterY = counterY + step)
                {
                    counterX++;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/right") && !_arguments[1].ToLower().Contains("click"))
            {
                for (int counter = point.X; counter < 3400; counter = counter + step)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/left"))
            {
                for (int counter = point.X; counter > 0; counter = counter - step)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/down"))
            {
                for (int counter = point.Y; counter < 1200; counter = counter + step)
                {
                    SetCursorPos(point.X, counter);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/up"))
            {
                for (int counter = point.Y; counter > 0; counter = counter - step)
                {
                    SetCursorPos(point.X, counter);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (_arguments[1].ToLower().Contains("/click"))
            {
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (_arguments[1].ToLower().Contains("/double-click"))
            {
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (_arguments[1].ToLower().Contains("/stop"))
            {
                KillAllMouseControls(processes);
            }
        }

        private static Process[] KillCurrentMouseControlProcess()
        {
            var processes = Process.GetProcessesByName("MouseControl");
            foreach (var process in processes)
            {
                var currentProcess = Process.GetCurrentProcess();
                if (process.Id != currentProcess.Id)
                {
                    process.Kill();
                }
            }

            return processes;
        }

        private void GetDelayFromSettings()
        {
            millisecondsDelay = Properties.Settings.Default.Delay;
        }

        private static void KillAllMouseControls(Process[] processes)
        {
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
    }
}
