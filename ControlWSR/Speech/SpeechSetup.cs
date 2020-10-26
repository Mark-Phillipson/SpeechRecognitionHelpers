using System;
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
			CreateDictationGrammar(speechRecogniser, "Shutdown", "Shutdown Windows");
			CreateDictationGrammar(speechRecogniser, "Short Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Begin Short Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Dictation", "Short Dictation");
			availableCommands = $"{availableCommands}\nShort Dictation";
			CreateDictationGrammar(speechRecogniser, "Testing", "Testing");
			CreateDictationGrammar(speechRecogniser, "Restart Dragon", "Restart Dragon");
			availableCommands = $"{availableCommands}\nRestart Dragon";
			BuildPhoneticAlphabetGrammars(speechRecogniser);
			availableCommands = $"{availableCommands}\n";

			LoadGrammarMouseCommands(speechRecogniser);
			availableCommands = $"{availableCommands}\nMOUSE COMMANDS";
			availableCommands = $"{availableCommands}\nPosition: Say < Left / Right > < Alpha - 7 > < Alpha - Tango > ";
			LoadGrammarMouseHorizontalPositionCommands(speechRecogniser);
			availableCommands = $"{availableCommands}\nPosition / Click: Say < Taskbar / Ribbon / Menu > < Alpha - 7 > ";


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
		public string SetupConfirmationCommands(string originalCommand,SpeechRecognizer speechRecogniser)
		{
			speechRecogniser.UnloadAllGrammars();
			CreateDictationGrammar(speechRecogniser, "Yes Please", "Confirmed");
			CreateDictationGrammar(speechRecogniser, "No Thank You", "Denied");
			var availableCommands = "";
			availableCommands = $"{availableCommands}\nYes Please";
			availableCommands = $"{availableCommands}\nNo Thank You";
			return availableCommands;
		}
		public void LoadGrammarMouseCommands(SpeechRecognizer speechRecognizer)
		{
			List<string> screenCoordinates = new List<string> { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Qubec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu", "1", "2", "3", "4", "5", "6", "7", "Zero" };
			Choices choices = new Choices();
			List<string> monitorNames = new List<string> { "Left", "Right", "Click", "Touch" };
			foreach (var item in screenCoordinates)
			{
				foreach (var monitorName in monitorNames)
				{
					foreach (var item2 in screenCoordinates)
					{
						if (item2 == "Uniform")
						{
							break;
						}
						choices.Add($"{monitorName} {item} {item2}");
					}
				}
			}
			GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
			Grammar grammar = new Grammar((GrammarBuilder)grammarBuilder);
			grammar.Name = "Mouse Command";
			speechRecognizer.LoadGrammarAsync(grammar);
		}
		public void LoadGrammarMouseHorizontalPositionCommands(SpeechRecognizer speechRecognizer)
		{
			List<string> screenCoordinates = new List<string> { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Qubec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu", "1", "2", "3", "4", "5", "6", "7", "Zero" };
			Choices choices = new Choices();
			List<string> horizontalPositions = new List<string> { "Taskbar", "Ribbon", "Menu" };
			foreach (var screenCoordinate in screenCoordinates)
			{
				foreach (var horizontalPosition in horizontalPositions)
				{
					choices.Add($"{horizontalPosition} {screenCoordinate}");
				}
			}
			GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
			Grammar grammar = new Grammar((GrammarBuilder)grammarBuilder);
			grammar.Name = "Horizontal Position Mouse Command";
			speechRecognizer.LoadGrammarAsync(grammar);
		}
		public void BuildPhoneticAlphabetGrammars( SpeechRecognizer speechRecogniser)
		{
			Choices phoneticAlphabet = new Choices(new string[] { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Qubec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu" });
			GrammarBuilder grammarBuilder2 = new GrammarBuilder();
			grammarBuilder2.Append(phoneticAlphabet);
			grammarBuilder2.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilder3 = new GrammarBuilder();
			grammarBuilder3.Append(phoneticAlphabet);
			grammarBuilder3.Append(phoneticAlphabet);
			grammarBuilder3.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilder4 = new GrammarBuilder();
			grammarBuilder4.Append(phoneticAlphabet);
			grammarBuilder4.Append(phoneticAlphabet);
			grammarBuilder4.Append(phoneticAlphabet);
			grammarBuilder4.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilder5 = new GrammarBuilder();
			grammarBuilder5.Append(phoneticAlphabet);
			grammarBuilder5.Append(phoneticAlphabet);
			grammarBuilder5.Append(phoneticAlphabet);
			grammarBuilder5.Append(phoneticAlphabet);
			grammarBuilder5.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilder6 = new GrammarBuilder();
			grammarBuilder6.Append(phoneticAlphabet);
			grammarBuilder6.Append(phoneticAlphabet);
			grammarBuilder6.Append(phoneticAlphabet);
			grammarBuilder6.Append(phoneticAlphabet);
			grammarBuilder6.Append(phoneticAlphabet);
			grammarBuilder6.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilder7 = new GrammarBuilder();
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			grammarBuilder7.Append(phoneticAlphabet);
			Choices phoneticAlphabet2to7 = new Choices(new GrammarBuilder[] { grammarBuilder2, grammarBuilder3, grammarBuilder4, grammarBuilder5, grammarBuilder6, grammarBuilder7 });
			Grammar grammarPhoneticAlphabets = new Grammar((GrammarBuilder)phoneticAlphabet2to7);
			grammarPhoneticAlphabets.Name = "Phonetic Alphabet";
			speechRecogniser.LoadGrammarAsync(grammarPhoneticAlphabets);
			Choices choicesLower = new Choices("Lower");
			BuildPhoneticAlphabetGrammars(speechRecogniser, phoneticAlphabet, choicesLower, "Phonetic Alphabet Lower");
			Choices choicesMixed = new Choices("Mixed");
			BuildPhoneticAlphabetGrammars( speechRecogniser, phoneticAlphabet, choicesMixed, "Phonetic Alphabet Mixed");
		}
		private static void BuildPhoneticAlphabetGrammars( SpeechRecognizer speechRecognizer, Choices phoneticAlphabet, Choices choices, string grammarName)
		{
			GrammarBuilder grammarBuilderLower2 = new GrammarBuilder();
			grammarBuilderLower2.Append(choices);
			grammarBuilderLower2.Append(phoneticAlphabet);
			grammarBuilderLower2.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilderLower3 = new GrammarBuilder();
			grammarBuilderLower3.Append(choices);
			grammarBuilderLower3.Append(phoneticAlphabet);
			grammarBuilderLower3.Append(phoneticAlphabet);
			grammarBuilderLower3.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilderLower4 = new GrammarBuilder();
			grammarBuilderLower4.Append(choices);
			grammarBuilderLower4.Append(phoneticAlphabet);
			grammarBuilderLower4.Append(phoneticAlphabet);
			grammarBuilderLower4.Append(phoneticAlphabet);
			grammarBuilderLower4.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilderLower5 = new GrammarBuilder();
			grammarBuilderLower5.Append(choices);
			grammarBuilderLower5.Append(phoneticAlphabet);
			grammarBuilderLower5.Append(phoneticAlphabet);
			grammarBuilderLower5.Append(phoneticAlphabet);
			grammarBuilderLower5.Append(phoneticAlphabet);
			grammarBuilderLower5.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilderLower6 = new GrammarBuilder();
			grammarBuilderLower6.Append(choices);
			grammarBuilderLower6.Append(phoneticAlphabet);
			grammarBuilderLower6.Append(phoneticAlphabet);
			grammarBuilderLower6.Append(phoneticAlphabet);
			grammarBuilderLower6.Append(phoneticAlphabet);
			grammarBuilderLower6.Append(phoneticAlphabet);
			grammarBuilderLower6.Append(phoneticAlphabet);
			GrammarBuilder grammarBuilderLower7 = new GrammarBuilder();
			grammarBuilderLower7.Append(choices);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			grammarBuilderLower7.Append(phoneticAlphabet);
			Choices phoneticAlphabetLower2to7 = new Choices(new GrammarBuilder[] { grammarBuilderLower2, grammarBuilderLower3, grammarBuilderLower4, grammarBuilderLower5, grammarBuilderLower6, grammarBuilderLower7 });
			Grammar grammarPhoneticAlphabets = new Grammar((GrammarBuilder)phoneticAlphabetLower2to7);
			grammarPhoneticAlphabets.Name = grammarName;
			speechRecognizer.LoadGrammarAsync(grammarPhoneticAlphabets);
		}
	}
}
