using ControlWSR.Speech;
using ControlWSR.Speech.Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace ControlWSR
{
	public partial class AvailableCommandsForm : Form
	{
		private readonly SpeechRecognizer speechRecogniser = new SpeechRecognizer();
		private readonly PerformVoiceCommands performVoiceCommands = new PerformVoiceCommands();
		private readonly SpeechSetup speechSetup = new SpeechSetup();
		private readonly InputSimulator inputSimulator = new InputSimulator();
		string textBoxResultsLocal;
		private string availableCommands;
		private string richTextBoxAvailableCommandsLocal;

		public string TextBoxResults
		{
			get => textBoxResultsLocal; set { textBoxResultsLocal = value; textBoxResults.Text = value; }
		}
		public string AvailableCommands { get => availableCommands; set { availableCommands = value; richTextBoxAvailableCommands.Text = value; } }
		public string RichTextBoxAvailableCommands { get => richTextBoxAvailableCommandsLocal; set { richTextBoxAvailableCommandsLocal = value; richTextBoxAvailableCommands.Text = value; } }
		public AvailableCommandsForm()
		{

			PerformVoiceCommands.ToggleSpeechRecognitionListeningMode(inputSimulator);
			InitializeComponent();
			speechRecogniser = speechSetup.StartWindowsSpeechRecognition();
			var availableCommands = speechSetup.SetUpMainCommands(speechRecogniser);
			richTextBoxAvailableCommands.Text = availableCommands;
			speechRecogniser.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizer_SpeechRecognised);
			speechRecogniser.SpeechRecognitionRejected += SpeechRecogniser_SpeechRecognitionRejected;
		}

		private void SpeechRecogniser_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			textBoxResults.Text = $"REJECTED: {e.Result.Grammar?.Name} {e.Result.Confidence:P} {DateTime.Now.TimeOfDay}";
		}

		private void SpeechRecognizer_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
		{
			textBoxResults.Text = "";
			var text = $"{e.Result.Text} {e.Result.Confidence:P}{Environment.NewLine}{Environment.NewLine}";
			foreach (var alternate in e.Result.Alternates)
			{
				text = text + $"{alternate.Text} {alternate.Confidence:P}{Environment.NewLine}";
			}
			textBoxResults.Text = text;
			performVoiceCommands.PerformCommand(e, this, speechRecogniser);
		}

		private void TestingBtn_Click(object sender, EventArgs e)
		{

		}

		private void AvailableCommandsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			PerformVoiceCommands.ToggleSpeechRecognitionListeningMode(inputSimulator);
			try
			{
				speechRecogniser.Enabled = false;
				speechRecogniser.Dispose();

			}
			catch (Exception)
			{
				// Just ignore
			}
			//inputSimulator.Keyboard.KeyDown(VirtualKeyCode.DIVIDE);
			//Thread.Sleep(1000);
			//inputSimulator.Keyboard.KeyUp(VirtualKeyCode.DIVIDE);
		}

		private void AvailableCommandsForm_Load(object sender, EventArgs e)
		{
			inputSimulator.Keyboard.KeyPress(VirtualKeyCode.DIVIDE);
		}
	}
}
