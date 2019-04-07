using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseControl
{
    public partial class MouseControlForm : Form
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);

        public struct POINT
        {
            public int X;
            public int Y;
        }
        public MouseControlForm()
        {
            InitializeComponent();
        }

        private void MouseControlForm_Load(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            POINT point;
            GetCursorPos(out point);

            string[] arguments = Environment.GetCommandLineArgs();
            //foreach (var argument in arguments)
            //{
            //    MessageBox.Show(argument);
            //}
            if (arguments[1].ToLower().Contains("/right"))
            {
                for (int counter = point.X; counter < 3400; counter++)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(50).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/left"))
            {
                for (int counter = point.X; counter > 0; counter--)
                {
                    SetCursorPos(counter, point.Y);
                    Task.Delay(50).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/down"))
            {
                for (int counter = point.Y; counter < 400; counter++)
                {
                    SetCursorPos(point.X,counter);
                    Task.Delay(50).Wait();
                }
            }
            else if (arguments[1].ToLower().Contains("/up"))
            {
                for (int counter = point.Y; counter> 0; counter--)
                {
                    SetCursorPos(point.X, counter);
                    Task.Delay(50).Wait();
                }
            }
            Application.Exit();
        }
    }
}
