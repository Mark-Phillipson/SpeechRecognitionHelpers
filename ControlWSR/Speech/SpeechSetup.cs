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
		SpeechCommandsHelper SpeechCommandsHelper = new SpeechCommandsHelper();
		public string SetUpMainCommands(SpeechRecognizer speechRecogniser)
		{
			speechRecogniser.UnloadAllGrammars();
			List<string> simpleCommands = new List<string>() {"yes","no", "Shutdown Windows", "Quit Application","Restart Windows","Restart Dragon","Show Recent","Fresh Line" };
			var availableCommands = "";
			foreach (var simpleCommand in simpleCommands)
			{
				availableCommands = $"{availableCommands}\n{simpleCommand}";
				CreateDictationGrammar(speechRecogniser, simpleCommand, simpleCommand);
			}
			CreateDictationGrammar(speechRecogniser, "Short Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Camel Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Title Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Variable Dictation", "Short Dictation");
			CreateDictationGrammar(speechRecogniser, "Upper Dictation", "Short Dictation");
			availableCommands = $"{availableCommands}\nShort Dictation";
			availableCommands = $"{availableCommands}\nCamel Dictation";
			availableCommands = $"{availableCommands}\nVariable Dictation";
			availableCommands = $"{availableCommands}\nTitle Dictation";
			CreateDictationGrammar(speechRecogniser, "Select Left", "Selection");
			CreateDictationGrammar(speechRecogniser, "Select Right", "Selection");
			CreateDictationGrammar(speechRecogniser, "Left Select", "Selection");
			CreateDictationGrammar(speechRecogniser, "Right Select", "Selection");
			CreateDictationGrammar(speechRecogniser, "Studio Command", "Studio Command");
			CreateDictationGrammar(speechRecogniser, "Command", "Studio Command");
			CreateDictationGrammar(speechRecogniser, "Studio", "Studio Command");
			BuildPhoneticAlphabetGrammars(speechRecogniser);
			LoadMoveCommandsGrammar(speechRecogniser);

			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Backspace", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Left", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Right", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Down", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Press Up", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Press Tab", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Delete", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Enter", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Press Page Down", "Repeat Keys", 30);
			SpeechCommandsHelper.CreateRepeatableCommand(speechRecogniser, "Press Page Up", "Repeat Keys", 30);
			availableCommands = $"{availableCommands}\n<1to30> Items";
			SpeechCommandsHelper.CreateItemCommands(speechRecogniser, "Items", "Select Items", 30);
			SetUpSymbolGrammarCommands(speechRecogniser);
			availableCommands = $"{availableCommands}\nSymbols In/Out";

			LoadGrammarMouseCommands(speechRecogniser);
			availableCommands = $"{availableCommands}\nMOUSE COMMANDS";
			availableCommands = $"{availableCommands}\nClick: Say <Click/Double-Click/Right Click/Mouse Click> ";
			CreateMouseMoveAndClickCommandGrammar(speechRecogniser);
			availableCommands = $"{availableCommands}\nPosition: Say < Left / Right > < Alpha - 7 > < Alpha - Tango > ";
			LoadGrammarMouseHorizontalPositionCommands(speechRecogniser);
			availableCommands = $"{availableCommands}\nPosition / Click: Say <Taskbar / Ribbon / Menu > < Alpha - 7 > ";
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
		private void CreateDictationGrammar(SpeechRecognizer speechRecognizer, string initialPhrase, string grammarName,bool openEnded=false)
		{
			GrammarBuilder grammarBuilder = new GrammarBuilder();
			grammarBuilder.Append(new Choices(initialPhrase));
			if (openEnded)
			{
				grammarBuilder.AppendDictation();
			}

			Grammar grammar = new Grammar((GrammarBuilder)grammarBuilder);
			grammar.Name = grammarName;
			speechRecognizer.LoadGrammarAsync(grammar);
		}
		public string SetupConfirmationCommands(string originalCommand,SpeechRecognizer speechRecogniser)
		{
			speechRecogniser.UnloadAllGrammars();
			CreateDictationGrammar(speechRecogniser, "Yes Please", "Confirmed");
			CreateDictationGrammar(speechRecogniser, "No Thank You", "Denied");
			var availableCommands = $"{originalCommand.ToUpper()}";
			availableCommands = $"{availableCommands}\n\nYes Please";
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

		public void LoadMoveCommandsGrammar(SpeechRecognizer speechRecognizer)
		{
			Choices choices = new Choices();
			choices.Add("Move Down");
			choices.Add("Move Up");
			choices.Add("Move Left");
			choices.Add("Move Right");
			for (int counter = 1; counter < 50; counter++)
			{
				choices.Add($"Move Down {counter}");
				choices.Add($"Move Up {counter}");
				choices.Add($"Move Left {counter}");
				choices.Add($"Move Right {counter}");
			}
			Grammar grammar = new Grammar(choices) { Name = "Move Command" };
			speechRecognizer.LoadGrammarAsync(grammar);
		}
		public void CreateMouseMoveAndClickCommandGrammar(SpeechRecognizer speechRecognizer)
		{
			Choices choices = new Choices();
			for (int counter = 1; counter < 100; counter++)
			{
				choices.Add($"Mouse Down {counter}");
				choices.Add($"Mouse Up {counter}");
				choices.Add($"Mouse Left {counter}");
				choices.Add($"Mouse Right {counter}");
			}
			for (int counter = 150; counter < 800; counter = counter + 50)
			{
				choices.Add($"Mouse Down {counter}");
				choices.Add($"Mouse Up {counter}");
				choices.Add($"Mouse Left {counter}");
				choices.Add($"Mouse Right {counter}");
			}
			Grammar grammar = new Grammar(choices);
			grammar.Name = "Mouse Move Command";
			speechRecognizer.LoadGrammarAsync(grammar);
			Choices choicesClick = new Choices();
			choicesClick.Add("Mouse Click");
			choicesClick.Add("Click");
			choicesClick.Add("Left Click");
			choicesClick.Add("Right Click");
			choicesClick.Add("Double Click");
			Grammar grammarClick = new Grammar(choicesClick);
			grammarClick.Name = "Mouse Click Command";
			speechRecognizer.LoadGrammarAsync(grammarClick);
		}
		public void SetUpSymbolGrammarCommands(SpeechRecognizer speechRecognizer)
		{
			Choices choices = new Choices();
			choices.Add("Square Brackets");
			choices.Add("Brackets");
			choices.Add("Curly Brackets");
			choices.Add("Single Quotes");
			choices.Add("Apostrophes");
			choices.Add("Quotes");
			choices.Add("At Signs");
			choices.Add("Chevrons");
			choices.Add("Equals");
			choices.Add("Not Equal");
			choices.Add("Plus");
			choices.Add("Dollar");
			choices.Add("Hash");
			choices.Add("Pipes");
			choices.Add("Ampersands");
			Choices choicesInOut = new Choices("In", "Out");
			GrammarBuilder grammarBuilder = new GrammarBuilder();
			grammarBuilder.Append(choices);
			grammarBuilder.Append(choicesInOut);
			Grammar grammarSymbols = new Grammar(grammarBuilder) { Name = "Symbols" };
			speechRecognizer.LoadGrammarAsync(grammarSymbols);
		}

	}
}
