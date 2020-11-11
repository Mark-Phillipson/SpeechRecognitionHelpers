using ControlWSR.Speech.Azure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace ControlWSR.Speech
{
	public class PerformVoiceCommands
	{
		readonly SpeechCommandsHelper SpeechCommandsHelper = new SpeechCommandsHelper();
		readonly InputSimulator inputSimulator = new InputSimulator();
		private readonly IEnumerable<VirtualKeyCode> all3Modifiers = new List<VirtualKeyCode>() { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT, VirtualKeyCode.MENU };
		private readonly IEnumerable<VirtualKeyCode> controlAndShift = new List<VirtualKeyCode>() { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT };
		private readonly IEnumerable<VirtualKeyCode> windowAndShift = new List<VirtualKeyCode>() { VirtualKeyCode.LWIN, VirtualKeyCode.SHIFT };
		public string CommandToBeConfirmed { get; set; } = null;
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName,
	string lpWindowName);

		// Activate an application window.
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		public Process currentProcess { get; set; }
		private readonly SpeechSetup speechSetup = new SpeechSetup();
		public PerformVoiceCommands()
		{
			UpdateCurrentProcess();
		}
		public async void PerformCommand(SpeechRecognizedEventArgs e, AvailableCommandsForm form, SpeechRecognizer speechRecogniser)
		{
			UpdateCurrentProcess();
			try
			{
				SpeechUI.SendTextFeedback(e.Result, $"Recognised: {e.Result.Text} {e.Result.Confidence:P1}", true);
			}
			catch (Exception)
			{
				//This will fail if were using the engine
			}
			if (e.Result.Grammar.Name == "Quit Application" && e.Result.Confidence > 0.6)
			{
				QuitApplication();
			}
			if (e.Result.Grammar.Name == "New with Space" && e.Result.Confidence > 0.6)
			{
				inputSimulator.Keyboard.TextEntry(" new ");
				inputSimulator.Keyboard.KeyDown(VirtualKeyCode.ESCAPE);
			}
			if (e.Result.Grammar.Name == "Window Monitor Switch" && e.Result.Confidence > 0.6)
			{
				inputSimulator.Keyboard.ModifiedKeyStroke(windowAndShift, VirtualKeyCode.RIGHT);
			}
			if (e.Result.Grammar.Name == "Select Line" && e.Result.Confidence > 0.6)
			{
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.HOME);
				inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.END);
			}
			if (e.Result.Grammar.Name == "Mouse Down" && e.Result.Confidence > 0.6)
			{
				inputSimulator.Mouse.LeftButtonDown();
			}
			else if (e.Result.Grammar.Name == "Shutdown Windows" && e.Result.Confidence > 0.5)
			{
				CommandToBeConfirmed = e.Result.Grammar.Name;
				SetupConfirmationCommands(speechRecogniser, form);
			}
			else if (e.Result.Grammar.Name == "Restart Windows" && e.Result.Confidence > 0.5)
			{
				CommandToBeConfirmed = e.Result.Grammar.Name;
				SetupConfirmationCommands(speechRecogniser, form);
			}
			else if (e.Result.Grammar.Name == "Confirmed")
			{
				if (CommandToBeConfirmed == "Shutdown Windows")
				{
					Process.Start("shutdown", "/s /t 10");
				}
				else if (CommandToBeConfirmed == "Restart Windows")
				{
					Process.Start("shutdown", "/r /t 10");
				}
				QuitApplication();
			}
			else if (e.Result.Grammar.Name == "Short Dictation" && e.Result.Confidence > 0.4)
			{
				await PerformShortDictation(e, form);
			}
			else if (e.Result.Grammar.Name == "Denied")
			{
				var availableCommands = speechSetup.SetUpMainCommands(speechRecogniser);
				form.RichTextBoxAvailableCommands = availableCommands;
			}
			else if (e.Result.Grammar.Name == "Restart Dragon" && e.Result.Confidence > 0.5)
			{
				RestartDragon();
			}
			else if (e.Result.Grammar.Name == "Studio Command" && e.Result.Confidence > 0.5)
			{
				RunVisualStudioCommand(speechRecogniser);
			}
			else if (e.Result.Grammar.Name == "Get and Set" && e.Result.Confidence > 0.5)
			{
				inputSimulator.Keyboard.TextEntry(" { get; set; }");
			}
			else if (e.Result.Grammar.Name == "Horizontal Position Mouse Command" && e.Result.Confidence > 0.3)
			{
				PerformHorizontalPositionMouseCommand(e);
			}
			else if (e.Result.Grammar.Name == "Mouse Command" && e.Result.Confidence > 0.3)
			{
				PerformMouseCommand(e);
			}
			else if (e.Result.Grammar.Name.Contains("Phonetic Alphabet" )) // Could be lower, mixed or upper
			{
				ProcessKeyboardCommand(e);
			}
			else if (e.Result.Grammar.Name == "Mouse Click Command" && e.Result.Confidence > 0.3)
			{
				PerformMouseClickCommand(e);
			}
			else if (e.Result.Grammar.Name == "Mouse Move Command" && e.Result.Confidence > 0.3)
			{
				PerformMouseMoveCommand(e);
			}
			else if (e.Result.Grammar.Name == "Repeat Keys" && e.Result.Confidence > 0.6)
			{
				List<string> keys= new List<string>();
				SpeechCommandsHelper.BuildRepeatSendkeys(e, keys);
				SendKeysCustom(null, null, keys, currentProcess.ProcessName);
			}
			else if (e.Result.Grammar.Name == "Go to Line" && e.Result.Confidence > 0.6)
			{
				var wordStartPosition = 3;
				if (e.Result.Text.StartsWith("Line"))
				{
					wordStartPosition = 1;
				}
				var lineNumber = "";
				for (int i = wordStartPosition; i < e.Result.Words.Count; i++)
				{
					lineNumber += e.Result.Words[i].Text + " ";
				}
				var numericLineNumberTest=  WordsToNumbers.ConvertToNumbers(lineNumber.ToString());
				lineNumber = numericLineNumberTest.ToString();
				//lineNumber = SpeechCommandsHelper.ConvertTextToNumber(lineNumber);
				bool isANumber = int.TryParse(lineNumber, out int numericLineNumber);
				if (isANumber)
				{
					inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_G);
					Thread.Sleep(100);
					inputSimulator.Keyboard.TextEntry(numericLineNumber.ToString());
					Thread.Sleep(100);
					inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
				}
			}
			else if (e.Result.Grammar.Name == "Select Items" && e.Result.Confidence > 0.6)
			{
				PerformSelectItemsCommand(e);
			}
			else if (e.Result.Grammar.Name == "Symbols" && e.Result.Confidence > 0.3)
			{
				PerformanceSymbolsCommand(e);
			}
			else if (e.Result.Grammar.Name == "Show Recent" && e.Result.Confidence > 0.5)
			{
				inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU,VirtualKeyCode.VK_F);
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_J);
			}
			else if (e.Result.Grammar.Name == "Fresh Line" && e.Result.Confidence > 0.5)
			{
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.END);
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
			}
			else if (e.Result.Grammar.Name == "Semi Colon" && e.Result.Confidence > 0.3)
			{
				inputSimulator.Keyboard.TextEntry(";");
			}
			else if (e.Result.Grammar.Name=="Selection" && e.Result.Confidence>0.5)
			{
				if (e.Result.Text.ToLower().Contains("left"))
				{
					inputSimulator.Keyboard.ModifiedKeyStroke(controlAndShift, VirtualKeyCode.LEFT);
				}
				else if (e.Result.Text.ToLower().Contains("right"))
				{
					inputSimulator.Keyboard.ModifiedKeyStroke(controlAndShift, VirtualKeyCode.RIGHT);
				}
			}
		}

		private void RunVisualStudioCommand(SpeechRecognizer speechRecogniser)
		{
			if (currentProcess.ProcessName == "devenv")
			{
				List<string> keys = new List<string>() { "%v" };
				SendKeysCustom(null, null, keys, currentProcess.ProcessName);
				ToggleSpeechRecognitionListeningMode(inputSimulator);
				Thread.Sleep(2000);
				ToggleSpeechRecognitionListeningMode(inputSimulator);
			}
			else
			{
				speechRecogniser.EmulateRecognize("Switch to Visual Studio");
			}

		}

		private async Task PerformShortDictation(SpeechRecognizedEventArgs e, AvailableCommandsForm form)
		{
			ToggleSpeechRecognitionListeningMode(inputSimulator);
			var result = await DictateSpeech.RecognizeSpeechAsync();
			form.TextBoxResults = result.Text;
			var rawResult = result.Text;
			rawResult = RemovePunctuation(rawResult);
			string[] stringSeparators = new string[] { " " };
			List<string> words = rawResult.Split(stringSeparators, StringSplitOptions.None).ToList();
			if (e.Result.Text.ToLower().Contains("camel"))
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
					rawResult = value;
				}
			}
			else if (e.Result.Text.ToLower().Contains("variable"))
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
				}
				rawResult = value;
			}
			else if (e.Result.Text.ToLower().Contains("dot notation"))
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + ".";
				}
				rawResult = value.Substring(0,value.Length-1);
			}
			else if (e.Result.Text.ToLower().Contains("title"))
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
				}
				rawResult = value;
			}
			else if (e.Result.Text.ToLower().StartsWith("upper"))
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.ToUpper() + " ";
				}
				rawResult = value;
			}
			else if (e.Result.Text.ToLower().StartsWith("lower"))
			{
				string value = "";
				foreach (var word in words)
				{
					value = value + word.ToLower() + " ";
				}
				rawResult = value;
			}
			if (!e.Result.Text.ToLower().StartsWith("short") && e.Result.Text.ToLower()!="dictation")
			{
				rawResult = rawResult.Trim();
			}
			if (rawResult.Length > 0)
			{
				inputSimulator.Keyboard.TextEntry(rawResult);
			}
			ToggleSpeechRecognitionListeningMode(inputSimulator);
		}

		private static string RemovePunctuation(string rawResult)
		{
			rawResult = rawResult.Replace(",", "");
			rawResult = rawResult.Replace(";", "");
			rawResult = rawResult.Replace(":", "");
			rawResult = rawResult.Replace("?", "");
			rawResult = rawResult.Replace(".", "");
			return rawResult;
		}

		private void PerformSelectItemsCommand(SpeechRecognizedEventArgs e)
		{
			var repeatCount = Int32.Parse(e.Result.Words[0].Text);
			inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
			for (int i = 0; i < repeatCount; i++)
			{
				inputSimulator.Keyboard.KeyPress(VirtualKeyCode.DOWN);
			}
			inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
		}

		private void RestartDragon()
		{
			var processName = "nsbrowse";
			KillAllProcesses(processName);
			processName = "dragonbar";
			KillAllProcesses(processName);
			processName = "natspeak";
			KillAllProcesses(processName);
			processName = "ProcHandler";
			KillAllProcesses(processName);
			processName = "KBPro";
			KillAllProcesses(processName);
			processName = "dragonlogger";
			KillAllProcesses(processName);
			try
			{
				Process process = new Process();
				var filename = "C:\\Program Files(x86)\\KnowBrainer\\KnowBrainer Professional 2017\\KBPro.exe";
				if (File.Exists(filename))
				{
					process.StartInfo.UseShellExecute = true;
					process.StartInfo.WorkingDirectory = "C:\\Program Files (x86)\\KnowBrainer\\KnowBrainer Professional 2017\\";
					process.StartInfo.FileName = filename;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
					process.Start();
				}
				else
				{
					IntPtr hwnd = GetForegroundWindow();
					uint pid;
					GetWindowThreadProcessId(hwnd, out pid);
					Process currentProcess = Process.GetProcessById((int)pid);
					List<string> keysKB = new List<string>(new string[] { "^+k" });
					SendKeysCustom(null, null, keysKB, currentProcess.ProcessName);
				}
			}
			catch (Exception exception)
			{
				System.Windows.Forms.MessageBox.Show(exception.Message);
			}
		}
		private void KillAllProcesses(string name)
		{
			var processName = (name);
			if (processName.Length > 0)
			{
				foreach (var process in Process.GetProcessesByName(processName))
				{
					try
					{
						process.Kill();
					}
					catch (Exception)
					{
						//System.Windows.MessageBox.Show(exception.Message);
					}
				}
			}
		}



		public static void ToggleSpeechRecognitionListeningMode(InputSimulator inputSimulator)
		{
			inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
			inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LWIN);
			inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
		}

		void SetupConfirmationCommands(SpeechRecognizer speechRecogniser, AvailableCommandsForm availableCommandsForm)
		{
			var availableCommands = speechSetup.SetupConfirmationCommands(CommandToBeConfirmed, speechRecogniser);
			availableCommandsForm.AvailableCommands = availableCommands;
		}
		private void UpdateCurrentProcess()
		{
			IntPtr hwnd = GetForegroundWindow();
			uint pid;
			GetWindowThreadProcessId(hwnd, out pid);
			currentProcess = Process.GetProcessById((int)pid);
		}
		private void QuitApplication()
		{
			inputSimulator.Keyboard.KeyDown(VirtualKeyCode.DIVIDE);
			try
			{
				System.Windows.Forms.Application.Exit();
			}
			catch (Exception exception)
			{
				System.Windows.MessageBox.Show(exception.Message, "Error trying to shut down", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void SendKeysCustom(string applicationClass, string applicationCaption, List<string> keys, string processName, string applicationToLaunch = "", int delay = 0)
		{
			// Get a handle to the application. The window class
			// and window name can be obtained using the Spy++ tool.
			IntPtr applicationHandle = IntPtr.Zero;
			while (true)
			{
				if (applicationClass != null || applicationCaption != null)
				{
					applicationHandle = FindWindow(applicationClass, applicationCaption);
				}

				// Verify that Application is a running process.
				if (applicationHandle == IntPtr.Zero)
				{
					if (applicationToLaunch != null && applicationToLaunch.Length > 0)
					{
						Process.Start(applicationToLaunch);
						Thread.Sleep(1000);
					}
					else
					{
						//       System.Windows.MessageBox.Show($"{applicationCaption} is not running.");
						//ActivateApp(processName);
						Process process = Process.GetProcessesByName(processName)[0];
						applicationHandle = process.Handle;
						break;
					}
				}
				else
				{
					break;
				}
			}

			// Make Application the foreground application and send it
			// a set of Keys.
			SetForegroundWindow(applicationHandle);
			foreach (var item in keys)
			{
				Thread.Sleep(delay);
				try
				{
					var temporary = item.Replace("(", "{(}");
					temporary = temporary.Replace(")", "{)}");

					SendKeys.SendWait(temporary);
				}
				catch (Exception exception)
				{
					System.Windows.MessageBox.Show(exception.Message, "Error trying to shut down", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}
		private void PerformHorizontalPositionMouseCommand(SpeechRecognizedEventArgs e)
		{
			Win32.POINT p = new Win32.POINT();
			p.x = 100;
			p.y = 100;
			var horizontalCoordinate = e.Result.Words[1].Text;
			if (horizontalCoordinate == "Zero")
			{
				p.x = 5;
			}
			else if (horizontalCoordinate == "Alpha")
			{
				p.x = 50;
			}
			else if (horizontalCoordinate == "Bravo")
			{
				p.x = 100;
			}
			else if (horizontalCoordinate == "Charlie")
			{
				p.x = 150;
			}
			else if (horizontalCoordinate == "Delta")
			{
				p.x = 200;
			}
			else if (horizontalCoordinate == "Echo")
			{
				p.x = 250;
			}
			else if (horizontalCoordinate == "Foxtrot")
			{
				p.x = 300;
			}
			else if (horizontalCoordinate == "Golf")
			{
				p.x = 350;
			}
			else if (horizontalCoordinate == "Hotel")
			{
				p.x = 400;
			}
			else if (horizontalCoordinate == "India")
			{
				p.x = 450;
			}
			else if (horizontalCoordinate == "Juliet")
			{
				p.x = 500;
			}
			else if (horizontalCoordinate == "Kilo")
			{
				p.x = 550;
			}
			else if (horizontalCoordinate == "Lima")
			{
				p.x = 600;
			}
			else if (horizontalCoordinate == "Mike")
			{
				p.x = 650;
			}
			else if (horizontalCoordinate == "November")
			{
				p.x = 700;
			}
			else if (horizontalCoordinate == "Oscar")
			{
				p.x = 750;
			}
			else if (horizontalCoordinate == "Papa")
			{
				p.x = 800;
			}
			else if (horizontalCoordinate == "Qubec")
			{
				p.x = 850;
			}
			else if (horizontalCoordinate == "Romeo")
			{
				p.x = 900;
			}
			else if (horizontalCoordinate == "Sierra")
			{
				p.x = 950;
			}
			else if (horizontalCoordinate == "Tango")
			{
				p.x = 1000;
			}
			else if (horizontalCoordinate == "Uniform")
			{
				p.x = 1050;
			}
			else if (horizontalCoordinate == "Victor")
			{
				p.x = 1100;
			}
			else if (horizontalCoordinate == "Whiskey")
			{
				p.x = 1150;
			}
			else if (horizontalCoordinate == "X-ray")
			{
				p.x = 1200;
			}
			else if (horizontalCoordinate == "Yankee")
			{
				p.x = 1250;
			}
			else if (horizontalCoordinate == "Zulu")
			{
				p.x = 1300;
			}
			else if (horizontalCoordinate == "1")
			{
				p.x = 1350;
			}
			else if (horizontalCoordinate == "2")
			{
				p.x = 1400;
			}
			else if (horizontalCoordinate == "3")
			{
				p.x = 1450;
			}
			else if (horizontalCoordinate == "4")
			{
				p.x = 1500;
			}
			else if (horizontalCoordinate == "5")
			{
				p.x = 1550;
			}
			else if (horizontalCoordinate == "6")
			{
				p.x = 1600;
			}
			else if (horizontalCoordinate == "7")
			{
				p.x = 1650;
			}
			if (e.Result.Words[0].Text == "Taskbar")
			{
				p.y = 1030;
			}
			else if (e.Result.Words[0].Text == "Ribbon" || e.Result.Words[0].Text == "Menu")
			{
				p.y = 85;
			}
			Win32.SetCursorPos(p.x, p.y);
			SpeechUI.SendTextFeedback(e.Result, $" {e.Result.Text} H{p.x} V{p.y}", true);
			Win32.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, 0);
		}
		private void PerformMouseCommand(SpeechRecognizedEventArgs e)
		{
			Win32.POINT p = new Win32.POINT();
			p.x = 100;
			p.y = 100;
			var horizontalCoordinate = e.Result.Words[1].Text;
			if (horizontalCoordinate == "Zero")
			{
				p.x = 5;
			}
			else if (horizontalCoordinate == "Alpha")
			{
				p.x = 50;
			}
			else if (horizontalCoordinate == "Bravo")
			{
				p.x = 100;
			}
			else if (horizontalCoordinate == "Charlie")
			{
				p.x = 150;
			}
			else if (horizontalCoordinate == "Delta")
			{
				p.x = 200;
			}
			else if (horizontalCoordinate == "Echo")
			{
				p.x = 250;
			}
			else if (horizontalCoordinate == "Foxtrot")
			{
				p.x = 300;
			}
			else if (horizontalCoordinate == "Golf")
			{
				p.x = 350;
			}
			else if (horizontalCoordinate == "Hotel")
			{
				p.x = 400;
			}
			else if (horizontalCoordinate == "India")
			{
				p.x = 450;
			}
			else if (horizontalCoordinate == "Juliet")
			{
				p.x = 500;
			}
			else if (horizontalCoordinate == "Kilo")
			{
				p.x = 550;
			}
			else if (horizontalCoordinate == "Lima")
			{
				p.x = 600;
			}
			else if (horizontalCoordinate == "Mike")
			{
				p.x = 650;
			}
			else if (horizontalCoordinate == "November")
			{
				p.x = 700;
			}
			else if (horizontalCoordinate == "Oscar")
			{
				p.x = 750;
			}
			else if (horizontalCoordinate == "Papa")
			{
				p.x = 800;
			}
			else if (horizontalCoordinate == "Qubec")
			{
				p.x = 850;
			}
			else if (horizontalCoordinate == "Romeo")
			{
				p.x = 900;
			}
			else if (horizontalCoordinate == "Sierra")
			{
				p.x = 950;
			}
			else if (horizontalCoordinate == "Tango")
			{
				p.x = 1000;
			}
			else if (horizontalCoordinate == "Uniform")
			{
				p.x = 1050;
			}
			else if (horizontalCoordinate == "Victor")
			{
				p.x = 1100;
			}
			else if (horizontalCoordinate == "Whiskey")
			{
				p.x = 1150;
			}
			else if (horizontalCoordinate == "X-ray")
			{
				p.x = 1200;
			}
			else if (horizontalCoordinate == "Yankee")
			{
				p.x = 1250;
			}
			else if (horizontalCoordinate == "Zulu")
			{
				p.x = 1300;
			}
			else if (horizontalCoordinate == "1")
			{
				p.x = 1350;
			}
			else if (horizontalCoordinate == "2")
			{
				p.x = 1400;
			}
			else if (horizontalCoordinate == "3")
			{
				p.x = 1450;
			}
			else if (horizontalCoordinate == "4")
			{
				p.x = 1500;
			}
			else if (horizontalCoordinate == "5")
			{
				p.x = 1550;
			}
			else if (horizontalCoordinate == "6")
			{
				p.x = 1600;
			}
			else if (horizontalCoordinate == "7")
			{
				p.x = 1650;
			}
			var verticalCoordinate = e.Result.Words[2].Text;
			if (verticalCoordinate == "Zero")
			{
				p.y = 5;
			}
			else if (verticalCoordinate == "Alpha")
			{
				p.y = 50;
			}
			else if (verticalCoordinate == "Bravo")
			{
				p.y = 100;
			}
			else if (verticalCoordinate == "Charlie")
			{
				p.y = 150;
			}
			else if (verticalCoordinate == "Delta")
			{
				p.y = 200;
			}
			else if (verticalCoordinate == "Echo")
			{
				p.y = 250;
			}
			else if (verticalCoordinate == "Foxtrot")
			{
				p.y = 300;
			}
			else if (verticalCoordinate == "Golf")
			{
				p.y = 350;
			}
			else if (verticalCoordinate == "Hotel")
			{
				p.y = 400;
			}
			else if (verticalCoordinate == "India")
			{
				p.y = 450;
			}
			else if (verticalCoordinate == "Juliet")
			{
				p.y = 500;
			}
			else if (verticalCoordinate == "Kilo")
			{
				p.y = 550;
			}
			else if (verticalCoordinate == "Lima")
			{
				p.y = 600;
			}
			else if (verticalCoordinate == "Mike")
			{
				p.y = 650;
			}
			else if (verticalCoordinate == "November")
			{
				p.y = 700;
			}
			else if (verticalCoordinate == "Oscar")
			{
				p.y = 750;
			}
			else if (verticalCoordinate == "Papa")
			{
				p.y = 800;
			}
			else if (verticalCoordinate == "Qubec")
			{
				p.y = 850;
			}
			else if (verticalCoordinate == "Romeo")
			{
				p.y = 900;
			}
			else if (verticalCoordinate == "Sierra")
			{
				p.y = 950;
			}
			else if (verticalCoordinate == "Tango")
			{
				p.y = 1000;
			}
			else if (verticalCoordinate == "Uniform")
			{
				p.y = 1050;
			}
			else if (verticalCoordinate == "Victor")
			{
				p.y = 1100;
			}
			else if (verticalCoordinate == "Whiskey")
			{
				p.y = 1150;
			}
			else if (verticalCoordinate == "X-ray")
			{
				p.y = 1200;
			}
			else if (verticalCoordinate == "Yankee")
			{
				p.y = 1250;
			}
			else if (verticalCoordinate == "Zulu")
			{
				p.y = 1300;
			}
			else if (verticalCoordinate == "1")
			{
				p.y = 1350;
			}
			else if (verticalCoordinate == "2")
			{
				p.y = 1400;
			}
			else if (verticalCoordinate == "3")
			{
				p.y = 1450;
			}
			else if (verticalCoordinate == "4")
			{
				p.y = 1500;
			}
			else if (verticalCoordinate == "5")
			{
				p.y = 1550;
			}
			else if (verticalCoordinate == "6")
			{
				p.y = 1600;
			}
			else if (verticalCoordinate == "7")
			{
				p.y = 1650;
			}
			var screen = e.Result.Words[0].Text;
			if (screen == "Right" || screen == "Touch")
			{
				p.x += 1680;
			}

			Win32.SetCursorPos(p.x, p.y);
			SpeechUI.SendTextFeedback(e.Result, $" {e.Result.Text} H{p.x} V{p.y}", true);
			if (screen == "Click" || screen == "Touch")
			{
				Win32.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, 0);
			}
		}
		private void ProcessKeyboardCommand(SpeechRecognizedEventArgs e)
		{
			var value = e.Result.Text;
			List<string> phoneticAlphabet = new List<string> { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa", "Qubec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey", "X-ray", "Yankee", "Zulu" };
			foreach (var item in phoneticAlphabet)
			{
				if (value.IndexOf("Shift") > 0)
				{
					value = value.Replace(item, item.ToUpper().Substring(0, 1));
				}
				else
				{
					value = value.Replace(item, item.ToLower().Substring(0, 1));
				}
			}
			value = value.Replace("Press ", "");
			value = value.Replace("Semicolon", ";");
			value = value.Replace("Control", "^");
			value = value.Replace("Alt Space", "% ");
			value = value.Replace("Alt", "%");
			value = value.Replace("Escape", "{Esc}");
			value = value.Replace("Zero", "0");
			value = value.Replace("Stop", ".");
			value = value.Replace("Tab", "{Tab}");
			value = value.Replace("Backspace", "{Backspace}");
			value = value.Replace("Enter", "{Enter}");
			value = value.Replace("Page Down", "{PgDn}");
			if (value.IndexOf("Page Up") >= 0)
			{
				value = value.Replace("Page Up", "{PgUp}");
			}
			else
			{
				value = value.Replace("Up", "{Up}");
			}
			value = value.Replace("Right", "{Right}");
			value = value.Replace("Left", "{Left}");
			value = value.Replace("Down", "{Down}");
			value = value.Replace("Delete", "{Del}");
			value = value.Replace("Home", "{Home}");
			value = value.Replace("End", "{End}");
			value = value.Replace("Hyphen", "-");
			value = value.Replace("Colon", ":");
			value = value.Replace("Ampersand", "&");
			value = value.Replace("Dollar", "$");
			value = value.Replace("Exclamation Mark", "!");
			value = value.Replace("Double Quote", "\"");
			value = value.Replace("Pound", "£");
			value = value.Replace("Asterix", "*");
			value = value.Replace("Apostrophe", "'");
			value = value.Replace("Equal", "=");
			value = value.Replace("Open Bracket", "(");
			value = value.Replace("Close Bracket", ")");


			for (int i = 12; i > 0; i--)
			{
				value = value.Replace($"Function {i}", "{F" + i + "}");
			}
			value = value.Replace("Shift", "+");
			if (value != "% ")
			{
				value = value.Replace(" ", "");
			}
			if (value.ToLower().Contains("space"))
			{
				value = value.ToLower().Replace("space", " ");
			}
			if (value.Contains("{Up}") && IsNumber(value.Substring(value.IndexOf("}") + 1)))
			{
				value = "{Up " + value.Substring(value.IndexOf("}") + 1) + "}";
			}
		if (value.Contains("{Down}") && IsNumber(value.Substring(value.IndexOf("}") + 1)))
			{
				value = "{Down " + value.Substring(value.IndexOf("}") + 1) + "}";
			}
			if (value.Contains("{Left}") && IsNumber(value.Substring(value.IndexOf("}") + 1)))
			{
				value = "{Left " + value.Substring(value.IndexOf("}") + 1) + "}";
			}
			if (value.Contains("{Right}") && IsNumber(value.Substring(value.IndexOf("}") + 1)))
			{
				value = "{Right " + value.Substring(value.IndexOf("}") + 1) + "}";
			}
			value = value.Replace("Percent", "{%}");
			value = value.Replace("Plus", "{+}");
			if (e.Result.Grammar.Name.Contains("Phonetic Alphabet"))
			{
				value = Get1stLetterFromPhoneticAlphabet(e, value);
			}

			//this.WriteLine($"*****Sending Keys: {value.Replace("{", "").Replace("}", "").ToString()}*******");

			List<string> keys = new List<string>(new string[] { value });
			SendKeysCustom(null, null, keys, currentProcess.ProcessName);
		}
		private static string Get1stLetterFromPhoneticAlphabet(SpeechRecognizedEventArgs e, string value)
		{
			if (e.Result.Grammar.Name == "Phonetic Alphabet")
			{
				value = "";
				foreach (var word in e.Result.Words)
				{
					if (word.Text!="Space")
					{
						value = value + word.Text.Substring(0, 1);
					}
					else
					{
						value += " ";
					}
				}
			}
			else if (e.Result.Grammar.Name == "Phonetic Alphabet Lower")
			{
				value = "";
				foreach (var word in e.Result.Words)
				{
					if (word.Text != "Lower")
					{
						if (word.Text != "Space")
						{
							value = value + word.Text.ToLower().Substring(0, 1);
						}
						else
						{
							value += " ";
						}
					}
				}
			}
			else if (e.Result.Grammar.Name == "Phonetic Alphabet Mixed")
			{
				value = "";var counter = 0;
				foreach (var word in e.Result.Words)
				{
					if (word.Text != "Mixed")
					{
						counter++;
						if (word.Text != "Space")
						{
							if (counter==1)
							{
								value = value + word.Text.ToUpper().Substring(0, 1);
							}
							else
							{
								value = value + word.Text.ToLower().Substring(0, 1);
							}
						}
						else
						{
							value += " ";
						}
					}
				}
			}
			else if (e.Result.Grammar.Name == "Replace Letters")
			{
				value = "";
				var upper = false;
				foreach (var word in e.Result.Words)
				{
					if (word.Text == "Upper")
					{
						upper = true;
					}
					else if (word.Text == "Replace" || word.Text == "With" || word.Text == "this")
					{
						//Do nothing
					}
					else
					{
						if (upper == true)
						{
							value = value + word.Text.ToUpper().Substring(0, 1);
							upper = false;
						}
						else
						{
							value = value + word.Text.ToLower().Substring(0, 1);
						}
					}
				}
			}
			return value;
		}
		public Boolean IsNumber(String value)
		{
			return value.All(Char.IsDigit);
		}
		private void PerformMouseMoveCommand(SpeechRecognizedEventArgs e)
		{
			Win32.POINT p = new Win32.POINT();
			Win32.GetCursorPos(out p);
			var direction = e.Result.Words[1].Text;
			var counter = int.Parse(e.Result.Words[2].Text);
			if (direction == "Down")
			{
				p.y = p.y + counter;
			}
			else if (direction == "Up")
			{
				p.y = p.y - counter;
			}
			else if (direction == "Left")
			{
				p.x = p.x - counter;
			}
			else if (direction == "Right")
			{
				p.x = p.x + counter;
			}
			Win32.SetCursorPos(p.x, p.y);
		}
		private void PerformMouseClickCommand(SpeechRecognizedEventArgs e)
		{
			Win32.POINT p = new Win32.POINT();
			Win32.GetCursorPos(out p);
			if (e.Result.Text == "Left Click" || e.Result.Text == "Mouse Click" || e.Result.Text == "Click")
			{
				Win32.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, 0);
			}
			else if (e.Result.Text == "Double Click")
			{
				Win32.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, 0);
				Win32.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.x, (uint)p.y, 0, 0);
			}
			else
			{
				Win32.mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.x, (uint)p.y, 0, 0);
			}
		}
		private void PerformanceSymbolsCommand(SpeechRecognizedEventArgs e)
		{
			List<string> keys = new List<string>();
			var text = e.Result.Text.ToLower();
			if (text.Contains("square brackets"))
			{
				keys.Add("[]");
			}
			else if (text.Contains("curly brackets"))
			{
				keys.Add("{{}");
				keys.Add("{}}");
			}
			else if (text.Contains("brackets"))
			{
				keys.Add("(");
				keys.Add(")");
			}
			else if (text.Contains("apostrophes"))
			{
				keys.Add("''");
			}
			else if (text.Contains("quotes"))
			{
				keys.Add("\"");
				keys.Add("\"");
			}
			else if (text.Contains("at signs"))
			{
				keys.Add("@@");
			}
			else if (text.Contains("chevrons"))
			{
				keys.Add("<>");
			}
			else if (text.Contains("equals"))
			{
				keys.Add("==");
			}
			else if (text.Contains("not equal"))
			{
				keys.Add("!=");
			}
			else if (text.Contains("plus"))
			{
				keys.Add("++");
			}
			else if (text.Contains("dollar"))
			{
				keys.Add("$$");
			}
			else if (text.Contains("hash"))
			{
				keys.Add("##");
			}
			else if (text.Contains("question marks"))
			{
				keys.Add("??");
			}
			else if (text.Contains("pipes"))
			{
				keys.Add("||");
			}
			SendKeysCustom(null, null, keys, currentProcess.ProcessName);
			if (text.EndsWith("in"))
			{
				List<string> keysLeft = new List<string> { "{Left}" };
				SendKeysCustom(null, null, keysLeft, currentProcess.ProcessName);
			}
		}
	}
}
