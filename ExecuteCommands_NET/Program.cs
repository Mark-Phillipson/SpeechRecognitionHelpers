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
				args = new string[] { "ExecuteCommands.exe", "natural", "open downloads" };
				Console.WriteLine("[DEBUG] No arguments detected. Defaulting to: natural 'open downloads'");
			}

			// Diagnostic: log raw args
			Console.WriteLine($"[DIAG] Raw args: [{string.Join(", ", args)}]");

			string modeRaw = args[1];
			string textRaw = args.Length > 2 ? string.Join(" ", args.Skip(2)) : "";
			string mode = modeRaw.TrimStart('/').Trim().ToLower();
			string text = textRaw.TrimStart('/').Trim();

			// Diagnostic: log normalized mode/text
			Console.WriteLine($"[DIAG] Normalized mode: '{mode}', text: '{text}'");

			if (string.IsNullOrWhiteSpace(mode))
			{
				Console.WriteLine("Error: Mode argument is empty. Usage: ExecuteCommands.exe <mode> <dictation>");
				return;
			}

			IHandleProcesses handleProcesses = new HandleProcesses();
			Commands commands = new Commands(handleProcesses);
			string result = "";
			// Log helper
			string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "app.log");
			logPath = System.IO.Path.GetFullPath(logPath);
			Console.WriteLine($"[DEBUG] Log file path: {logPath}"); // Diagnostic: print log path
			void Log(string message)
			{
				try
				{
					System.IO.File.AppendAllText(logPath, $"{DateTime.Now}: {message}\n");
				}
				catch(Exception exception) {System.Console.WriteLine(exception.Message); }
			}

			Log($"Args: {string.Join(", ", args)}");
			Log($"ModeRaw: {modeRaw}, TextRaw: {textRaw}");
			Log($"Normalized Mode: {mode}, Text: {text}");
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

			Log($"Result: {result}");
			Console.WriteLine(result);

		}
	}
}