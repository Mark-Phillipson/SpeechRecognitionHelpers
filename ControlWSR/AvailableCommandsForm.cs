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
		private string _rejected;

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
			speechRecogniser.SpeechHypothesized += SpeechRecogniser_SpeechHypothesized;
			speechRecogniser.StateChanged += SpeechRecogniser_StateChanged;
		}

		private void SpeechRecogniser_StateChanged(object sender, StateChangedEventArgs e)
		{
			this.textBoxResults.Text ="State has changed to:" + e.RecognizerState.ToString();
		}

		private void SpeechRecogniser_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
		{

				this.textBoxResults.Text = "Hypothesized. I'm listening... (" + e.Result.Text + " " + Math.Round(e.Result.Confidence * 100) + "%)";
			if (string.IsNullOrEmpty(_rejected))
			{
				if (e.Result.Text != "yes" && e.Result.Confidence > 0.5F)
				{
					_rejected = e.Result.Text;
					this.textBoxResults.Text = "Confirm.  Did you mean " + e.Result.Text + "? (say yes or no)";
				}
			}

		}

		private void SpeechRecogniser_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			if (e.Result.Text != "yes" && e.Result.Confidence > 0.5F)
			{
				_rejected = e.Result.Text;
				this.textBoxResults.Text = "Rejected.  Did you mean " + e.Result.Text + "? (say yes or no)";
			}
		}

		private void SpeechRecognizer_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
		{

			speechRecogniser.PauseRecognizerOnRecognition = true;
			if (e.Result.Grammar.Name=="no")
			{
				_rejected = null;
				return;
			}
			if (e.Result.Grammar.Name=="yes")
			{
				speechRecogniser.EmulateRecognizeAsync(_rejected);
				_rejected = null;
				return;
			}

			textBoxResults.Text = "";
			var text = $"{e.Result.Text} {e.Result.Confidence:P}{Environment.NewLine}{Environment.NewLine}";
			foreach (var alternate in e.Result.Alternates)
			{
				text += $"{alternate.Text} {alternate.Confidence:P}{Environment.NewLine}";
			}
			textBoxResults.Text = text;
			performVoiceCommands.PerformCommand(e, this, speechRecogniser);
			try
			{
				speechRecogniser.PauseRecognizerOnRecognition = false;
			}
			catch (Exception)
			{
				// Ignore this error
			}
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
		}

		private void AvailableCommandsForm_Load(object sender, EventArgs e)
		{
			inputSimulator.Keyboard.KeyPress(VirtualKeyCode.DIVIDE);
			BackColor = Color.FromArgb(38, 38, 38);
			ForeColor = Color.White;
			FontFamily fontFamily = new FontFamily("Cascadia Code");
			Font font = new Font(fontFamily, (float)11, FontStyle.Bold, GraphicsUnit.Point);
			richTextBoxAvailableCommands.Font = font;
			richTextBoxAvailableCommands.BackColor = Color.FromArgb(38,38,38);
			richTextBoxAvailableCommands.ForeColor = Color.White;
			textBoxResults.Font = font;
			textBoxResults.BackColor = Color.Black;
			textBoxResults.ForeColor = Color.White;
		}
	}
}
