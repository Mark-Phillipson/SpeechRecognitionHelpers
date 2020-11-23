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
		public async Task<RecognitionResult> RecognizeSpeechAsync(SpeechRecognizer recogniser)
		{
				var result = await recogniser.RecognizeOnceAsync();
				if (result.Reason == ResultReason.RecognizedSpeech)
				{
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

