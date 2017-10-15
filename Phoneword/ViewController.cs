using System;

using UIKit;


//Speech recognition
using Speech;
using Foundation;
using AVFoundation;

//API client functions 
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Collections.Generic;

//our namespaces
using Phoneword.Models;

namespace Phoneword
{
    public partial class ViewController : UIViewController
    {
        private SFSpeechAudioBufferRecognitionRequest recognitionRequest;
        private SFSpeechRecognitionTask recognitionTask;
        private AVAudioEngine audioEngine = new AVAudioEngine();
        private SFSpeechRecognizer speechRecognizer = new SFSpeechRecognizer(new NSLocale("en_US"));

        private Kpi relatedKpi;
        private List<Kpi> neededKpi;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
           
            SpeakerButton.Enabled = false;

            Querybox.Text = "";
            Querybox.Placeholder = "Your question...";

           
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus auth) =>
            {
                bool buttonIsEnabled = false;
                switch (auth)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        buttonIsEnabled = true;
                        var node = audioEngine.InputNode;
                        var recordingFormat = node.GetBusOutputFormat(0);
                        node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) =>
                        {
                            recognitionRequest.Append(buffer);
                        });
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        buttonIsEnabled = false;
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                        buttonIsEnabled = false;
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                        buttonIsEnabled = false;
                        break;
                }

                InvokeOnMainThread(() => { SpeakerButton.Enabled = buttonIsEnabled; });
            });

            //Event triggered when the button is pressed
            SpeakerButton.TouchUpInside += delegate
            {
                //MY BAE
                //https://www.grapecity.com/en/blogs/how-to-use-speech-recognition-with-xamarin-ios
                if (audioEngine.Running == true)
                {
                    StopRecording();
                    Listener.Text = "Stopped";
                    SpeakerButton.Highlighted = false;
                }
                else
                {
                    StartRecording();
                    Listener.Text = "Started";
                    SpeakerButton.Highlighted = true;
                }
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void HomeDeleteButton_TouchUpInside(UIButton sender)
        {
            Querybox.Text = "";
            Querybox.Placeholder = "Your question...";
        }

        //BAE number 2
        //https://developer.xamarin.com/guides/ios/getting_started/hello,_iOS_multiscreen/hello,_iOS_multiscreen_quickstart/
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            base.PrepareForSegue(segue, sender);

            var kpiViewController = segue.DestinationViewController as KPIViewController;

            //============= Calling our API ======
            // will probably move this to a client class as well. but fuck it for now 
            string dealer_name = "omega";
            relatedKpi = new Kpi();
            try
            {
                string url;
                HttpClient client;
                HttpResponseMessage response;
                string json_string;
                
                client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                //add any default headers below this
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                //grabbing related kpi
                url = $"{BASE_URL}RelatedKpi?query={Uri.EscapeDataString(Querybox.Text)}&dealer_name={dealer_name}";
                response = client.GetAsync(url).Result;
                json_string = response.Content.ReadAsStringAsync().Result;

                relatedKpi = JsonConvert.DeserializeObject<Kpi>(json_string);
                if(relatedKpi != null)
                {
                    kpiViewController.relatedKpi = relatedKpi;
                }

                //grabbing needed kpis
                url = $"{BASE_URL}NeededKpi?dealer_name={dealer_name}";
                response = client.GetAsync(url).Result;
                json_string = response.Content.ReadAsStringAsync().Result;

                neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);
                if (relatedKpi != null)
                {
                    kpiViewController.neededKpi = neededKpi;
                }

            }
            catch (Exception e)
            {
                new UIAlertView("API Error", "Error with VDA API call: " + e.Message, null, "OK", null).Show();
            }

        }



        // ============== Speech Recognition Functions ============
        //  probably want to store in class or something in the future
        public void StartRecording()
        {
            Querybox.Placeholder = "Recording";
            recognitionRequest = new SFSpeechAudioBufferRecognitionRequest();

            audioEngine.Prepare();
            NSError error;
            audioEngine.StartAndReturnError(out error);
            if (error != null)
            {
                Console.WriteLine(error.ToString());
                return;
            }
            recognitionTask = speechRecognizer.GetRecognitionTask(recognitionRequest, (SFSpeechRecognitionResult result, NSError err) =>
            {
                if (err != null)
                {
                    Console.WriteLine(err.ToString());
                }
                else
                {
                    if (result.Final == true)
                    {
                        Querybox.Text = result.BestTranscription.FormattedString;

                    }
                }
            });
        }

        public void StopRecording()
        {
            audioEngine.Stop();
            recognitionRequest.EndAudio();
        }


    }
}