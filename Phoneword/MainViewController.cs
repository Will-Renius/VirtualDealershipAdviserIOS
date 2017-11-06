﻿using System;

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
using CoreGraphics;

namespace Phoneword
{
    public partial class MainViewController : UIViewController
    {
        private SFSpeechAudioBufferRecognitionRequest recognitionRequest;
        private SFSpeechRecognitionTask recognitionTask;
        private AVAudioEngine audioEngine = new AVAudioEngine();
        private SFSpeechRecognizer speechRecognizer = new SFSpeechRecognizer(new NSLocale("en_US"));

        private Kpi relatedKpi;                 //Kpi most closely matched to dealer question
        public string dealer_name;

        public List<Kpi> neededKpi;            //List of dealer's worst KPI
        private UIView activeview;             // Controller that activates the keyboard
        private System.nfloat scroll_amount = 0.0f; //Amount of screen that will scroll
        private System.nfloat bottom = 0.0f;    //Button point of the screen

        private float offset = 10.0f;          // Extra offset
        private bool moveViewUp = false;           //Whether the view moves up (depends on keyboard)



        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        private void KeyBoardUpNotification(NSNotification notification)
        {//Moves the screen up, when the keyboard shows

            //Getting the keyboards size
            var val = (NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameEndUserInfoKey);
            CGRect r = val.CGRectValue;

            // Find what view opened the keyboard
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeview = view;
            }

            // Bottom of the controller = initial position + height + offset      
            bottom = (activeview.Frame.Y + activeview.Frame.Height + offset);

            // Calculate how far we need to scroll
            scroll_amount = (r.Height - (View.Frame.Size.Height - bottom));

            // Perform the scrolling
            if (scroll_amount > 0)
            {
                moveViewUp = true;
                ScrollTheView(moveViewUp);
            }
            else
            {
                moveViewUp = false;
            }

        }

        private void KeyBoardDownNotification(NSNotification notification)
        {//Moves screen down when keyboard is dismissed
            if (moveViewUp) { ScrollTheView(false); }
        }


        private void ScrollTheView(bool move)
        {//Scroll view up or down

            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            CGRect frame = View.Frame;

            if (move)
            {
                frame.Y -= scroll_amount;
            }
            else
            {
                frame.Y += scroll_amount;
                scroll_amount = 0;
            }

            View.Frame = frame;
            UIView.CommitAnimations();
        }

        public override void ViewDidLoad()
        {
            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false; //for iOS5

            View.AddGestureRecognizer(g);

            /*this.EnterLabel.ShouldReturn += (textField) => {
                textField.ResignFirstResponder();
                return true;
            };*/


            //UIView _topKeyboard = new UIView();
            //UIButton _done = new UIButton();

            //_topKeyboard.Add(_done);

            //this.InputAccessoryView = _topKeyboard;

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardDownNotification);


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
                    SpeakerButton.Highlighted = false;
                }
                else
                {
                    StartRecording();
                    Querybox.Placeholder = "Listening...";
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
            Querybox.Placeholder = "Your question:";
        }

        //https://developer.xamarin.com/guides/ios/getting_started/hello,_iOS_multiscreen/hello,_iOS_multiscreen_quickstart/
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            //string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            string BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/"; //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< New Post
            base.PrepareForSegue(segue, sender);

            var kpiViewController = segue.DestinationViewController as KPIViewController;

            //we gotta reset our list variables
            kpiViewController.neededKpi = new List<Kpi>();
            neededKpi = new List<Kpi>();

            //============= Calling our API ======
            // will probably move this to a client class as well
            //dealer_name = "omega";, replaced with sending dealer name from LoginController
            relatedKpi = new Kpi();

            string url, json_string, query;
            HttpClient client;
            HttpResponseMessage response = new HttpResponseMessage();

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            //add any default headers below this
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //grabbing related kpi
            if (string.IsNullOrEmpty(Querybox.Text))
            {
                new UIAlertView("Query Error", "No query, defaulting to \"dealer insell\"...", null, "OK", null).Show();
                query = "how is my insell";
            }
            else
            {
                query = Querybox.Text;
            }

            url = $"{BASE_URL}RelatedKpi?query={Uri.EscapeDataString(query)}&dealer_name={dealer_name}";
            //url = $"{BASE_URL}RelatedKpi?query={Uri.EscapeDataString(query)}&dealer_name=Beta";
            //Is the RelatedKpi different for each dealer

            //give'er 3 tries!
            for (int i = 0; i < 3; i++)
            {
                response = client.GetAsync(url).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    //some other stuff?
                    break;
                }
                else
                {
                    //BUG: name = Dealer Share doesnt work in api
                    //some stuff probably
                    continue;
                }
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                new UIAlertView("GET /RelatedKpi ERR:", response.StatusCode.ToString(), null, "OK", null).Show();

                relatedKpi = new Kpi { name = "ERR: Please consider redoing question..." };
            }
            else
            {
                json_string = response.Content.ReadAsStringAsync().Result;
                //Says it expects json string to be a kpi model
                relatedKpi = JsonConvert.DeserializeObject<Kpi>(json_string);

                new UIAlertView("Returning related KPI\n", $"Here ya go: \"{relatedKpi.name + relatedKpi.p_val.ToString()}\"", null, "OK", null).Show();

                if (relatedKpi == null)
                {
                    new UIAlertView("Deserialization ERR", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();

                    relatedKpi = new Kpi { name = "ERR: Please consider redoing question..." };
                }
            }
            kpiViewController.relatedKpi = relatedKpi;

            //grabbing needed kpis
            url = $"{BASE_URL}NeededKpi?dealer_name={dealer_name}";
            response = client.GetAsync(url).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                new UIAlertView("GET /NeededKpi ERR:", response.StatusCode.ToString(), null, "OK", null).Show();

                neededKpi.Add(new Kpi { name = "Error finding your needed KPIs" });
            }
            else
            {
                json_string = response.Content.ReadAsStringAsync().Result;
                neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                if (neededKpi == null)
                {
                    new UIAlertView("Deserialization ERR", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                    neededKpi.Add(new Kpi { name = "Error deserializing your needed kpis" });
                }
            }

            kpiViewController.neededKpi = neededKpi;

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