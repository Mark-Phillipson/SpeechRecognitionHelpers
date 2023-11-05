using ExecuteCommands;

namespace ExecuteCommands_NET
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			//ApplicationConfiguration.Initialize();
			//Application.Run(new Form1());
			string[] args = Environment.GetCommandLineArgs();
			//MessageBox.Show("got to line eighteen of the program");
			IHandleProcesses handleProcesses = new HandleProcesses();
			Commands commands = new Commands(handleProcesses);
			var result = commands.PerformCommand(args);

			Console.WriteLine(result);

		}
	}
}