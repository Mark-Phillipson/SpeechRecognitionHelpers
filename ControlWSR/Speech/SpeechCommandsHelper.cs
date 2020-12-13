using System.Collections.Generic;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace ControlWSR.Speech
{
	public partial class SpeechCommandsHelper
	{

		public void CreateRepeatableCommand(SpeechRecognizer speechRecognizer, string spokenCommand, string grammarName = null, int maximumRepeat = 10)
		{
			Choices choices = new Choices();
			for (int counter = 1; counter < maximumRepeat; counter++)
			{
				choices.Add($"{spokenCommand} {counter}");
			}
			Grammar grammar = new Grammar(choices);
			if (grammarName == null)
			{
				grammarName = spokenCommand;
			}
			grammar.Name = grammarName;
			speechRecognizer.LoadGrammarAsync(grammar);
		}
		public void CreateItemCommands(SpeechRecognizer speechRecognizer, string spokenCommand, string grammarName = null, int maximumRepeat = 10)
		{
			Choices choices = new Choices();
			for (int counter = 1; counter < maximumRepeat; counter++)
			{
				choices.Add($"{counter} {spokenCommand}");
			}
			Grammar grammar = new Grammar(choices);
			if (grammarName == null)
			{
				grammarName = spokenCommand;
			}
			grammar.Name = grammarName;
			speechRecognizer.LoadGrammarAsync(grammar);
		}

		public static void BuildRepeatSendkeys(SpeechRecognizedEventArgs e, List<string> keys)
		{
			int numberIndexPosition = 1;
			if (e.Result.Text.ToUpper().Contains("STEP OVER"))
			{
				numberIndexPosition = 2;
			}
				var counter = int.Parse(e.Result.Words[numberIndexPosition].Text);
			var sendkeysCommand = $"{{{e.Result.Words[0].Text.ToUpper()}}}";
			if (e.Result.Text.ToUpper().Contains("PAGE UP"))
			{
				sendkeysCommand = "{PGUP}";
			}
			else if (e.Result.Text.ToUpper().Contains("PAGE DOWN"))
			{
				sendkeysCommand = "{PGDN}";
			}
			else if (e.Result.Text.ToUpper().Contains("STEP OVER"))
			{
				sendkeysCommand = "{F10}";
			}
			sendkeysCommand = sendkeysCommand.Replace("Press", "");
			for (int i = 0; i < counter; i++)
			{
				keys.Add(sendkeysCommand);
			}
		}
		public string ConvertTextToNumber(string lineNumber)
		{
			var possible2 = new List<string> { "two", "to", "too" };
			var possible4 = new List<string> { "four", "for"};
			lineNumber = lineNumber.ToLower();
			if (lineNumber=="one")
			{
				return "1";
			}
			else if (possible2.Contains(lineNumber))
			{
				return "2";
			}
			else if (lineNumber=="three")
			{
				return "3";
			}
			else if (possible4.Contains(lineNumber))
			{
				return "4";
			}
			else if (lineNumber == "five")
			{
				return "5";
			}
			else if (lineNumber == "six")
			{
				return "6";
			}
			else if (lineNumber == "seven")
			{
				return "7";
			}
			else if (lineNumber == "eight")
			{
				return "8";
			}
			else if (lineNumber == "nine")
			{
				return "9";
			}
			else if (lineNumber == "ten")
			{
				return "10";
			}
			return lineNumber;
		}

	}
}
