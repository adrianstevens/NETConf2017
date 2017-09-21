using System;
using System.Diagnostics;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace SayHelloXF
{
	public class TextToSpeechService : ITextToSpeech
	{
		public async void Speak(string text)
		{
            var mediaPronunciation = new MediaElement();
            try
            {
                using (var speech = new SpeechSynthesizer())
                {
                    speech.Voice = SpeechSynthesizer.DefaultVoice;
                    var voiceStream = await speech.SynthesizeTextToStreamAsync(text);
                    mediaPronunciation.SetSource(voiceStream, voiceStream.ContentType);
                    mediaPronunciation.Play();
                }
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}
	}
}

