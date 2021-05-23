using DictationBoxMSP;
using DNSTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteCommands
{
    public static class SpeakMessage
    {
		public static void TextToSpeech(string messageToSpeak)
		{

			DgnMicBtn gDgnMic = new DgnMicBtn();
			gDgnMic.Register(0);

			try
			{
				((IDgnMicBtn)gDgnMic).MicState = DgnMicStateConstants.dgnmicOff;

				SpeechSynthesizer speechSynthesiser = new SpeechSynthesizer();
				speechSynthesiser.Speak(messageToSpeak);
				speechSynthesiser.Dispose();

			}

			catch (Exception exception)
			{
				DisplayMessage displayMessage =  new DisplayMessage($"Dragons DgnMicBtn failed when turning the microphone off. The error message is: { Environment.NewLine} { exception.Message}");
			}
			try
			{
				((IDgnMicBtn)gDgnMic).MicState = DgnMicStateConstants.dgnmicOn;
			}

			catch (Exception exception)
			{
				DisplayMessage displayMessage = new DisplayMessage($"Dragons DgnMicBtn failed when turning the microphone ON. The error message is: { Environment.NewLine} { exception.Message}");
			}
			gDgnMic.UnRegister();
		}
	}
}
