using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                arguments = new string[] { args[0], "ToggleMicrophone" };
                //arguments=  new string[] { args[0], "/upper-left" };
            }
            else
            {
                arguments = Environment.GetCommandLineArgs();
            }
            if (arguments[1].Contains("CloseFileExplorer"))
            {
                CloseFileExplorer();
                Console.WriteLine("Close File Explorer Ran successfully. ");
            }
            else if (arguments[1].Contains ("ShowDictationBox"))
            {
                ShowDictationBox();
                Console.WriteLine("The Show Dictation Command ran successfully!");
            }
            else if (arguments[1].Contains("ToggleMicrophone"))
            {
                ToggleMicrophone();
                Console.WriteLine("The toggle microphone command executed successfully!");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
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
        private void ShowDictationBox()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                //This is where we can launch the dictation box
                var filename = @"C:\Program Files (x86)\MSP Systems\Speech Recognition Helpers\DictationBoxMSP.exe";
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.WorkingDirectory = "C:\\Program Files (x86)\\KnowBrainer\\KnowBrainer Professional 2017\\";
                process.StartInfo.FileName = filename;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.Start();
            }
        }
        private void ToggleMicrophone()
        {
            SendKeys.SendWait("{ADD}");
        }

    }
}
