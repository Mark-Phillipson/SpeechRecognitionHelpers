using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseControl
{
    public class MouseControl
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event
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
        public void PerformControl()
        {
        int millisecondsDelay = Properties.Settings.Default.Delay;
        var processes = Process.GetProcessesByName("MouseControl");
            foreach (var process in processes)
            {
                var currentProcess = Process.GetCurrentProcess();
                if (process.Id != currentProcess.Id)
                {
                    process.Kill();
                }
            }
            POINT point;
            GetCursorPos(out point);

            string[] arguments;
            //foreach (var argument in arguments)
            //{
            //    MessageBox.Show(argument);
            //}
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count()<2)
            {
                arguments=  new string[] { args[0], "/25" };
                //arguments=  new string[] { args[0], "/upper-left" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            //Debug.Print(arguments[1]);
            int delay = 0;
            if (Int32.TryParse(arguments[1].Substring(1), out delay))
            {
                Properties.Settings.Default.Delay = delay;
                Properties.Settings.Default.Save();
                millisecondsDelay = Properties.Settings.Default.Delay;
                //MessageBox.Show(arguments[1] + " delay" + millisecondsDelay);
            }
            if (arguments[1].ToLower().Contains("/right-click"))
            {
                mouse_event((int)MouseEventType.RightDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.RightUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (arguments[1].ToLower().Contains("/upper-left"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY > 0; counterY--)
                {
                    counterX--;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/upper-right"))
            {
                int counterX = point.X; 
                for (int counterY = point.Y; counterY > 0; counterY--)
                {
                    counterX++;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/lower-left"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY < 1200; counterY++)
                {
                    counterX--;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/lower-right"))
            {
                int counterX = point.X;
                for (int counterY = point.Y; counterY < 1200; counterY++)
                {
                    counterX++;
                    SetCursorPos(counterX, counterY);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/right") && !arguments[1].ToLower().Contains("click"))
            {
                for (int counter = point.X; counter < 3400; counter++)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/left"))
            {
                for (int counter = point.X; counter > 0; counter--)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/down"))
            {
                for (int counter = point.Y; counter < 1200; counter++)
                {
                    SetCursorPos(point.X, counter);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/up"))
            {
                for (int counter = point.Y; counter > 0; counter--)
                {
                    SetCursorPos(point.X, counter);
                    Task.Delay(millisecondsDelay).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/click"))
            {
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (arguments[1].ToLower().Contains("/double-click"))
            {
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftDown, point.X, point.Y, 0, 0);
                mouse_event((int)MouseEventType.LeftUp, point.X, point.Y, 0, 0);
                KillAllMouseControls(processes);
            }
            else if (arguments[1].ToLower().Contains("/stop"))
            {
                KillAllMouseControls(processes);
            }
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
