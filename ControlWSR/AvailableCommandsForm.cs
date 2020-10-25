using ControlWSR.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ControlWSR
{
	public partial class AvailableCommandsForm : Form
	{
		private readonly SpeechRecognizer speechRecogniser = new SpeechRecognizer();
		private readonly PerformVoiceCommands performVoiceCommands = new PerformVoiceCommands();
		private readonly SpeechSetup speechSetup = new SpeechSetup();
		public AvailableCommandsForm()
		{
			InitializeComponent();
			speechRecogniser= speechSetup.StartWindowsSpeechRecognition();
			var availableCommands=speechSetup.SetUpMainCommands(speechRecogniser);
			richTextBoxAvailableCommands.Text = availableCommands;
			speechRecogniser.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizer_SpeechRecognised);

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
			performVoiceCommands.PerformCommand(e);
		}
	}
}
