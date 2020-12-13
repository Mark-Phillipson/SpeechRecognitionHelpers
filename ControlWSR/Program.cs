using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlWSR
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new AvailableCommandsForm());
			//Console.WriteLine("Azure Cosmos Table Samples");
			//BasicSamples basicSamples = new BasicSamples();
			//basicSamples.RunSamples().Wait();

			//AdvancedSamples advancedSamples = new AdvancedSamples();
			//advancedSamples.RunSamples().Wait();

			//Console.WriteLine();
			//Console.WriteLine("Press any key to exit");
			//Console.Read();

		}
	}
}
