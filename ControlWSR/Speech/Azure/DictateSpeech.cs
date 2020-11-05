using System.Configuration;
using System.Collections.Specialized;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWSR.Speech.Azure
{
	public class DictateSpeech
	{
		public static async Task<RecognitionResult> RecognizeSpeechAsync()
		{
			string SPEECH__SERVICE__KEY;
			string SPEECH__SERVICE__REGION;
			SPEECH__SERVICE__KEY = ConfigurationManager.AppSettings.Get("SpeechAzureKey");
			SPEECH__SERVICE__REGION = ConfigurationManager.AppSettings.Get("SpeechAzureRegion");
			var config = SpeechConfig.FromSubscription(SPEECH__SERVICE__KEY, SPEECH__SERVICE__REGION);
			System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(@"C:\Users\MPhil\Source\Repos\SpeechRecognitionHelpers\ControlWSR\Media\Start.wav");
			soundPlayer.Play();
			using (var recogniser = new SpeechRecognizer(config))
			{
				var result = await recogniser.RecognizeOnceAsync();
				if (result.Reason == ResultReason.RecognizedSpeech)
				{
					//soundPlayer.SoundLocation=@"C:\Users\MPhil\Source\Repos\SpeechRecognitionHelpers\ControlWSR\Media\End.wav";
					//soundPlayer.Play();
					return result;
				}
				else if (result.Reason == ResultReason.NoMatch)
				{
					return result;
				}
				else if (result.Reason == ResultReason.Canceled)
				{
					var cancellation = CancellationDetails.FromResult(result);
					var returnResult= $"CANCELED: Reason={cancellation.Reason}";

					if (cancellation.Reason == CancellationReason.Error)
					{
						returnResult=$"{returnResult} CANCELED: ErrorCode={cancellation.ErrorCode}";
						returnResult=$"{returnResult} CANCELED: ErrorDetails={cancellation.ErrorDetails}";
						returnResult=$"{returnResult} CANCELED: Did you update the subscription info?";
					}
					return result;
				}
				return result;
			}
		}
	}
}

