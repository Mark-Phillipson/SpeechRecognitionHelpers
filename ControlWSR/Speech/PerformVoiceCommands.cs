using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows;
using System.Windows.Forms;

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
		public PerformVoiceCommands()
		{
			UpdateCurrentProcess();
		}

		public void PerformCommand(SpeechRecognizedEventArgs e)
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
			else if (e.Result.Grammar.Name=="Shutdown Windows" && e.Result.Confidence>0.5)
			{
				ShutdownWindows();
			}
		}
		void ShutdownWindows()
		{
			if (System.Windows.MessageBox.Show("Please Confirm","Shutdown Windows",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.Yes)==MessageBoxResult.Yes)
			{
				Process.Start("shutdown", "/s /t 0");
			}
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
				System.Windows.MessageBox.Show(exception.Message, "Error trying to shut down", MessageBoxButton.OK,MessageBoxImage.Error);
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
