﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ControlWSR.Speech
{
    public class SpeechSetup
    {
		public string SetUpMainCommands(SpeechRecognizer speechRecogniser)
		{
			var availableCommands = "Quit Application";
			speechRecogniser.UnloadAllGrammars();
			Choices choices = new Choices();
			choices.Add($"quit application");
			availableCommands = $"{availableCommands}\nShutdown Windows";
			CreateDictationGrammar(speechRecogniser, "Shut down Windows", "Shutdown Windows");
			CreateDictationGrammar(speechRecogniser, "Shutdown Windows", "Shutdown Windows");
			CreateDictationGrammar(speechRecogniser, "Testing", "Testing");
			Grammar grammar = new Grammar(new GrammarBuilder(choices));
			speechRecogniser.LoadGrammarAsync(grammar);
			return availableCommands;
		}
		public SpeechRecognizer StartWindowsSpeechRecognition()
		{
			try
			{
				SpeechRecognizer speechRecognizer = new SpeechRecognizer();
				return speechRecognizer;
			}
			catch (Exception exception)
			{
				System.Windows.MessageBox.Show($"Error loading Windows Speech Recognition {exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return  null ;
			}
		}
		private void CreateDictationGrammar(SpeechRecognizer speechRecognizer, string initialPhrase, string grammarName)
		{
			GrammarBuilder grammarBuilder = new GrammarBuilder();
			grammarBuilder.Append(new Choices(initialPhrase));
			grammarBuilder.AppendDictation();

			Grammar grammar = new Grammar((GrammarBuilder)grammarBuilder);
			grammar.Name = grammarName;
			speechRecognizer.LoadGrammarAsync(grammar);
		}

	}
}
