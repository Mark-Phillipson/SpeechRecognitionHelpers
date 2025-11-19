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
			// -------------------------------------------------------------
			// CLI contract:
			//   ExecuteCommands.exe <mode> <dictation>
			//     <mode>: 'natural', 'sharp', or other string
			//     <dictation>: free-form text to interpret or execute
			//
			// Examples:
			//   ExecuteCommands.exe natural "move this window to the other screen"
			//   ExecuteCommands.exe sharp "Jump to Symbol"
			// -------------------------------------------------------------
			string[] args = Environment.GetCommandLineArgs();

			// DEBUG: If no arguments, default to natural mode and sample dictation
			if (args.Length < 2)
			{
				args = new string[] { "ExecuteCommands.exe", "natural", "move this window to the other screen" };
				Console.WriteLine("[DEBUG] No arguments detected. Defaulting to: natural 'move this window to the other screen'");
			}

			string mode = args[1].ToLower();
			string text = args.Length > 2 ? string.Join(" ", args.Skip(2)) : "";

			if (string.IsNullOrWhiteSpace(mode))
			{
				Console.WriteLine("Error: Mode argument is empty. Usage: ExecuteCommands.exe <mode> <dictation>");
				return;
			}

			IHandleProcesses handleProcesses = new HandleProcesses();
			Commands commands = new Commands(handleProcesses);
			string result = "";
			switch (mode)
			{
				case "natural":
					result = commands.HandleNaturalAsync(text);
					break;
				case "sharp":
					result = commands.PerformCommand(new string[] { mode, text });
					break;
				default:
					// For now, treat unknown modes as natural
					result = commands.HandleNaturalAsync(text);
					break;
			}

			Console.WriteLine(result);

		}
	}
}