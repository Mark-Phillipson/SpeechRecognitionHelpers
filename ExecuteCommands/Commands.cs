using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteCommands
{
    class Commands
    {
        public void PerformCommand()
        {
            string[] arguments;
            //foreach (var argument in arguments)
            //{
            //    MessageBox.Show(argument);
            //}
            string[] args = Environment.GetCommandLineArgs();
            if (args.Count() < 2)
            {
                arguments = new string[] { args[0], "CloseFileExplorer" };
                //arguments=  new string[] { args[0], "/upper-left" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            if (arguments[1]=="CloseFileExplorer")
            {
                CloseFileExplorer();
            }
        }

        private void CloseFileExplorer()
        {
            //var test = Process.GetProcesses();
            var processes = Process.GetProcessesByName("explorer");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
    }
}
