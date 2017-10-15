using System;
using UIKit;
using Speech;
using Foundation;
using AVFoundation;

namespace Phoneword
{
    class Recorder
    {
        private AVAudioEngine AudioEngine;
        private SFSpeechRecognizer SpeechRecognizer;
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest;
        private SFSpeechRecognitionTask RecognitionTask;

        public Recorder()
        {
            AudioEngine = new AVAudioEngine();
            SpeechRecognizer = new SFSpeechRecognizer();
            LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        }

        private string input { get; set; }
        public string Input
        {
            get { return input; }
        }

        public void StartRecording()
        {
            // Setup audio session
            var node = AudioEngine.InputNode;
            node.RemoveTapOnBus(0);
            var recordingFormat = node.GetBusOutputFormat(0);
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) => {
                // Append buffer to recognition request
                LiveSpeechRequest.Append(buffer);
            });

            // Start recording
            AudioEngine.Prepare();
            NSError error;
            AudioEngine.StartAndReturnError(out error);

            // Did recording start?
            if (error != null)
            {
                // Handle error and return
                input = "Error: Recording init error!";
            }

            // Start recognition
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) => {
                // Was there an error?
                if (err != null)
                {
                    // Handle error
                    input = "Error: Recognition task";
                }
                else
                {
                    // Is this the final translation?
                    if (result.Final)
                    {
                        input = result.BestTranscription.ToString();

                        new UIAlertView("DONE", "You said \"{" + input + "}\".", null, "OK", null).Show();
                    }
                }
            });
        }

        public void StopRecording()
        {
            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();
        }

        public void CancelRecording()
        {
            AudioEngine.Stop();
            RecognitionTask.Cancel();
        }
    }
}
