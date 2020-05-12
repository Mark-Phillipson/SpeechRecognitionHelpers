using System;

namespace ExecuteCommands
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            IHandleProcesses handleProcesses = new HandleProcesses();
            Commands commands = new Commands(handleProcesses);
            var result = commands.PerformCommand(args);
            Console.WriteLine(result);
        }
    }
}
