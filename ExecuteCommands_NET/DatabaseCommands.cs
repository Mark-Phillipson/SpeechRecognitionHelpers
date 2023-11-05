using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using DataAccessLibrary.Models;
using ExecuteCommands.Repositories;
using SpeechContinuousRecognition;

namespace ExecuteCommands
{
	 public  class DatabaseCommands
	{
		IInputSimulator inputSimulator = new InputSimulator();
		WindowsVoiceCommand windowsVoiceCommand = new WindowsVoiceCommand();
		public (bool commandRun, string? commandName, string? errorMessage) PerformDatabaseCommands(string resultRaw, string? applicationName)
		{
			bool commandRun = false;
			string? commandName = null;
			string? dictation = null;
			WindowsSpeechVoiceCommand? command = null;
			command = windowsVoiceCommand.GetCommand(resultRaw, applicationName);
			if (command == null)
			{
				var specialCommands = windowsVoiceCommand.GetSpecialCommands(applicationName, "<dictation>");
				foreach (var dictationCommand in specialCommands)
				{
					if (dictationCommand.SpokenForms != null)
					{
						foreach (var item in dictationCommand.SpokenForms)
						{
							string spokenForm = item.SpokenFormText.Replace("<dictation>", "").Trim();
							if (resultRaw.ToLower().StartsWith(spokenForm.ToLower().Trim()))
							{
								command = dictationCommand;
								dictation = resultRaw.ToLower().Replace(spokenForm.ToLower(), "").Trim();
								break;
							}
							if (command != null)
							{
								break;
							}
						}
					}
				}
			}
			if (command == null)
			{
				string? enterCommand = windowsVoiceCommand.GetEnterCommand(resultRaw);
				if (enterCommand != null && enterCommand.Length > 0)
				{
					inputSimulator.Keyboard.TextEntry(enterCommand);
					commandRun = true;
					commandName = $"Enter **********";
					return (commandRun, commandName, null);
				}
			}
			if (command != null)
			{
				List<CustomWindowsSpeechCommand>? actions = windowsVoiceCommand.GetChildActions(command.Id);
				if (actions == null) { return (commandRun, "Nothing", " No action record found "); }
				foreach (var action in actions)
				{
					if (dictation != null)
					{
						dictation = FormatDictation(dictation, action.HowToFormatDictation);
					}
					if (action.WaitTime > 0)
					{
						Thread.Sleep(action.WaitTime);
					}
					if (action.MethodToCall != null && action.MethodToCall.Length > 0)
					{
						object[]? objects = new object[1];
						objects[0] = dictation ?? "";
						CustomMethods customMethods = new CustomMethods();
						Type thisType = customMethods.GetType();
						MethodInfo? theMethod = thisType.GetMethod(action.MethodToCall);
						if (theMethod != null)
						{
							string? methodResult = theMethod.Invoke(customMethods, objects) as string;
						}
					}
					if (action.KeyDownValue != VirtualKeyCode.NONAME && action.KeyDownValue!= null )
					{
						inputSimulator.Keyboard.KeyDown((VirtualKeyCode)action.KeyDownValue );
					}
					if (!string.IsNullOrWhiteSpace(action.TextToEnter))
					{
						string textEntry = action.TextToEnter.Replace("<dictation>", dictation);
						if (action.TextToEnter.Contains("<clipboard>"))
						{
							string clipboard = Clipboard.GetText();
							textEntry = action.TextToEnter.Replace("<clipboard>", clipboard);
						}

						if (textEntry != null && textEntry.Length > 0)
						{
							inputSimulator.Keyboard.TextEntry(textEntry);
						}
					}
					if (action.ControlKey || action.AlternateKey || action.ShiftKey || action.WindowsKey)
					{
						var modifiers = new List<VirtualKeyCode>();
						if (action.ControlKey)
						{
							modifiers.Add(VirtualKeyCode.CONTROL);
						}
						if (action.AlternateKey)
						{
							modifiers.Add(VirtualKeyCode.MENU);
						}
						if (action.ShiftKey)
						{
							modifiers.Add(VirtualKeyCode.SHIFT);
						}
						if (action.WindowsKey)
						{
							modifiers.Add(VirtualKeyCode.LWIN);
						}
						if (action.KeyPressValue != VirtualKeyCode.NONAME && action.KeyPressValue!=  null )
						{
							inputSimulator.Keyboard.ModifiedKeyStroke(modifiers,(VirtualKeyCode) action.KeyPressValue);
						}
					}
					else if (action.KeyPressValue != VirtualKeyCode.NONAME && action.KeyPressValue != null)
					{
						inputSimulator.Keyboard.KeyPress((VirtualKeyCode)action.KeyPressValue);
					}
					if (action.KeyUpValue != VirtualKeyCode.NONAME && action.KeyUpValue != null)
					{
						inputSimulator.Keyboard.KeyUp((VirtualKeyCode)action.KeyUpValue);
					}
					if (action.MouseCommand == "LeftButtonDown")
					{
						ModifierProcessing(action, true, inputSimulator);
						inputSimulator.Mouse.LeftButtonDown();

					}
					else if (action.MouseCommand == "RightButtonDown")
					{
						inputSimulator.Mouse.RightButtonDown();
					}
					else if (action.MouseCommand == "LeftButtonDoubleClick")
					{
						inputSimulator.Mouse.LeftButtonDoubleClick();
					}
					else if (action.MouseCommand == "RightButtonDoubleClick")
					{
						inputSimulator.Mouse.RightButtonDoubleClick();
					}
					else if (action.MouseCommand == "LeftButtonUp")
					{
						ModifierProcessing(action, false, inputSimulator);
						inputSimulator.Mouse.LeftButtonUp();

					}
					else if (action.MouseCommand == "RightButtonUp")
					{
						inputSimulator.Mouse.RightButtonUp();
					}
					else if (action.MouseCommand == "MiddleButtonClick")
					{
						//inputSimulator.Mouse.MiddleButtonClick();
					}
					else if (action.MouseCommand == "MiddleButtonDoubleClick")
					{
						//inputSimulator.Mouse.MiddleButtonDoubleClick();
					}
					else if (action.MouseCommand == "HorizontalScroll")
					{
						inputSimulator.Mouse.HorizontalScroll(action.ScrollAmount);
					}
					else if (action.MouseCommand == "VerticalScroll")
					{
						inputSimulator.Mouse.VerticalScroll(action.ScrollAmount);
					}
					else if (action.MouseCommand == "MoveMouseBy")
					{
						inputSimulator.Mouse.MoveMouseBy(action.MouseMoveX, action.MouseMoveY);
					}
					else if (action.MouseCommand == "MoveMouseTo")
					{
						inputSimulator.Mouse.MoveMouseTo(action.AbsoluteX, action.AbsoluteY);
					}
					if (!string.IsNullOrWhiteSpace(action.ProcessStart) && !string.IsNullOrWhiteSpace(action.CommandLineArguments))
					{
						try
						{
							Process.Start(action.ProcessStart.Replace("<dictation>", dictation), action.CommandLineArguments.Replace("<dictation>", dictation));
						}
						catch (Exception exception)
						{
							commandRun = true;
							return (commandRun, commandName, $"{{{exception.Message}}}");
						}
					}
					else if (!string.IsNullOrWhiteSpace(action.ProcessStart))
					{
						if (action.ProcessStart.StartsWith("http"))
						{
							var psi = new System.Diagnostics.ProcessStartInfo();
							psi.UseShellExecute = true;
							psi.FileName = action.ProcessStart;
							System.Diagnostics.Process.Start(psi);
						}
						else
						{
							try
							{
								Process.Start(action.ProcessStart.Replace("<dictation>", dictation));
							}
							catch (Exception exception)
							{
								commandRun = false;
								return (commandRun, commandName, $"{{{exception.Message}}}");
							}
						}
						commandRun = true;
						return (commandRun, commandName, "");
					}
					if (!string.IsNullOrWhiteSpace(action.SendKeysValue))
					{
						try
						{
							action.SendKeysValue = action.SendKeysValue.Replace("<dictation>", dictation);
							SendKeys.SendWait(action.SendKeysValue);
						}
						catch (Exception exception)
						{
							commandName = $"Exception has occurred: ({exception.Message})";
							commandRun = true;
							return (commandRun, commandName, null);
						}
					}
					commandRun = true;
					var spokenForm = command?.SpokenForms?.FirstOrDefault();
					if (spokenForm != null)
					{
						commandName = spokenForm.SpokenFormText;
					}
				}
			}
			string[] stringSeparators = new string[] { " " };
			List<string> words = resultRaw.Split(stringSeparators, StringSplitOptions.None).ToList();
			if (resultRaw.ToLower().StartsWith("add tag"))
			{
				PerformHtmlTagsInsertion(inputSimulator, resultRaw);
				commandRun = true;
				commandName = "Add Tag";
			}
			if (resultRaw.ToLower().StartsWith("find ") && applicationName == "Visual Studio")
			{
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.HOME);
				inputSimulator.Keyboard.Sleep(100);
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SHIFT, VirtualKeyCode.RIGHT);
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_F);
				inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
				//SendKeys.SendWait("^f");
				inputSimulator.Keyboard.Sleep(200);
				var searchTerm = "";
				var counter = 0;

				foreach (var word in words)
				{
					if (counter >= 2)
					{
						searchTerm = $"{searchTerm} {word.Replace(".", "")}";
					}
					counter++;
				}
				if (!string.IsNullOrWhiteSpace(searchTerm))
				{
					inputSimulator.Keyboard.TextEntry(searchTerm.Trim());
				}
				inputSimulator.Keyboard.Sleep(100);
				if (words[1].ToLower() == "previous")
				{
					inputSimulator.Keyboard.Sleep(100);
					inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SHIFT, VirtualKeyCode.F3);
					inputSimulator.Keyboard.Sleep(100);
					inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
				}
				else
				{
					inputSimulator.Keyboard.Sleep(100);
					inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
				}
				commandRun = true;
				commandName = $"{resultRaw} {searchTerm}";
			}
			return (commandRun, commandName, null);

		}
		private string FormatDictation(string dictation, string howToFormatDictation)
		{
			if (howToFormatDictation == "Do Nothing")
			{
				return dictation;
			}
			string[] stringSeparators = new string[] { " " };

			string result = "";
			List<string> words = dictation.Split(stringSeparators, StringSplitOptions.None).ToList();
			if (howToFormatDictation == "Camel")
			{
				var counter = 0; string value = "";
				foreach (var word in words)
				{
					counter++;
					if (counter != 1)
					{
						value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
					}
					else
					{
						value += word.ToLower();
					}
					result = value;
				}
			}
			else if (howToFormatDictation == "Variable")
			{
				string value = "";
				foreach (var word in words)
				{
					if (word.Length > 0)
					{
						value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
					}
				}
				result = value;
			}
			else if (howToFormatDictation == "dot notation")
			{
				string value = "";
				foreach (var word in words)
				{
					if (word.Length > 0)
					{
						value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + ".";
					}
				}
				result = value.Substring(0, value.Length - 1);
			}
			else if (howToFormatDictation == "Title")
			{
				string value = "";
				foreach (var word in words)
				{
					if (word.Length > 0)
					{
						value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
					}
				}
				result = value;
			}
			else if (howToFormatDictation == "Upper")
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.ToUpper() + " ";
				}
				result = value;
			}
			else if (howToFormatDictation == "Lower")
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.ToLower() + " ";
				}
				result = value;
			}
			return result;
		}
		private void ModifierProcessing(CustomWindowsSpeechCommand action, bool keyDown, IInputSimulator inputSimulator)
		{
			if (action.ControlKey && keyDown)
			{
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
			}
			if (action.ShiftKey && keyDown)
			{
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
			}
			if (action.AlternateKey && keyDown)
			{
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.MENU);
			}
			if (action.WindowsKey && keyDown)
			{
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LWIN);
			}
			if (action.ControlKey && !keyDown)
			{
				inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
			}
			if (action.ShiftKey && !keyDown)
			{
				inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
			}
			if (action.AlternateKey && !keyDown)
			{
				inputSimulator.Keyboard.KeyUp(VirtualKeyCode.MENU);
			}
			if (action.WindowsKey && !keyDown)
			{
				inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LWIN);
			}
		}
		private void PerformHtmlTagsInsertion(  IInputSimulator inputSimulator, string resultRaw)
		{
			string[] stringSeparators = new string[] { " " };
			List<string> words = resultRaw.Split(stringSeparators, StringSplitOptions.None).ToList();

			var tag = "";
			var counter = 0;
			foreach (var word in words)
			{
				if (counter >= 2)
				{
					tag = $"{tag} {word}";
				}
				counter++;
			}
			tag = RemovePunctuation(tag);
			var result = windowsVoiceCommand.GetHtmlTag(tag.Trim());
			if (result == null)
			{
				return;
			}
			string? tagReturned = result.ListValue?.ToLower();
			string textToType = ""; int moveLeft = 0;
			if (tagReturned == "input" || tagReturned == "br" || tagReturned == "hr")
			{
				textToType = $"<{tagReturned} />";
				moveLeft = 3;
			}
			else if (tagReturned == "img")
			{
				textToType = $"<{tagReturned} src='' />";
				moveLeft = 5;
			}
			else
			{
				textToType = $"<{tagReturned}>";
				moveLeft = 1;// + tagReturned!.Length;
			}
			inputSimulator.Keyboard.TextEntry(textToType);
			for (int i = 1; i < moveLeft; i++)
			{
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LEFT);
				inputSimulator.Keyboard.Sleep(100);
			}
		}
		public static string RemovePunctuation(string rawResult)
		{
			rawResult = rawResult.Replace(",", "");
			rawResult = rawResult.Replace(";", "");
			rawResult = rawResult.Replace(":", "");
			rawResult = rawResult.Replace("?", "");
			if (rawResult.EndsWith(".")) { rawResult = rawResult.Substring(startIndex: 0, length: rawResult.Length - 1); }
			return rawResult;
		}

	}
}
