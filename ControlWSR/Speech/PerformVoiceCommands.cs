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
//using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace ControlWSR.Speech
{
	public class PerformVoiceCommands
	{
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
			if (e.Result.Text.ToLower() == "quit application" && e.Result.Confidence > 0.6)
			{
				QuitApplication();
			}
			else if (e.Result.Grammar.Name == "Shutdown Windows" && e.Result.Confidence > 0.5)
			{
				ShutdownWindows(speechRecogniser, form);
			}
			else if (e.Result.Grammar.Name == "Short Dictation" && e.Result.Confidence > 0.4)
			{
				InputSimulator inputSimulator = new InputSimulator();
				ToggleSpeechRecognitionListeningMode(inputSimulator);
				var result = await DictateSpeech.RecognizeSpeechAsync();
				form.TextBoxResults = result.Text;
				if (result.Text.Length>0)
				{
					inputSimulator.Keyboard.TextEntry(result.Text);
				}
				ToggleSpeechRecognitionListeningMode(inputSimulator);
			}
			else if (e.Result.Grammar.Name == "Confirmed")
			{
				Process.Start("shutdown", "/s /t 0");
			}
			else if (e.Result.Grammar.Name=="Denied")
			{
				var availableCommands = speechSetup.SetUpMainCommands(speechRecogniser);
				form.RichTextBoxAvailableCommands = availableCommands;
			}
			else if (e.Result.Grammar.Name=="Restart Dragon" && e.Result.Confidence>0.5)
			{
				RestartDragon();
			}
		}

		public static void ToggleSpeechRecognitionListeningMode(InputSimulator inputSimulator)
		{
			inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
			inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LWIN);
			inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
		}

		void ShutdownWindows(SpeechRecognizer speechRecogniser, AvailableCommandsForm availableCommandsForm)
		{
			var availableCommands = speechSetup.SetupConfirmationCommands("Shutdown Windows", speechRecogniser);
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
			List<string> keys = new List<string>(new string[] { "{DIVIDE}" });
			SendKeysCustom(null, null, keys, currentProcess.ProcessName);
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

	}
}
