using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace ControlWSR.Speech
{
	public class SpeechCommandsHelper
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
			var counter = int.Parse(e.Result.Words[1].Text);
			var sendkeysCommand = $"{{{e.Result.Words[0].Text.ToUpper()}}}";
			if (e.Result.Text.ToUpper().Contains("PAGE UP"))
			{
				sendkeysCommand = "PGUP";
			}
			else if (e.Result.Text.ToUpper().Contains("PAGE DOWN"))
			{
				sendkeysCommand = "PGDN";
			}
			sendkeysCommand = sendkeysCommand.Replace("Press", "");
			for (int i = 0; i < counter; i++)
			{
				keys.Add(sendkeysCommand);
			}
		}

	}
}
