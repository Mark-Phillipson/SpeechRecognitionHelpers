using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace VoiceLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string thisProcessName = Process.GetCurrentProcess().ProcessName;
            //if (Process.GetProcesses().Count(p => p.ProcessName == thisProcessName)>1) 
            //{
            //    return;
            //}
            OpenForms openForms = new OpenForms();
            openForms.LoadForm();
        }
    }
}
