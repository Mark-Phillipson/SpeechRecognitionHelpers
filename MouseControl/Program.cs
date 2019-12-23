using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MouseControlForm());
            string[] arguments;
            if (args.Count() < 2)
            {
                arguments = new string[] { args[0], "/1" };
                //arguments=  new string[] { args[0], "/upper-left" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            MouseControl mouseControl = new MouseControl(arguments);
            mouseControl.PerformControl();
        }
    }
}
