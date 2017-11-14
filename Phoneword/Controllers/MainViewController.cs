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
using CoreGraphics;
using Phoneword.Gateways;

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

        private VDAGateway vdaGateway;

        string final_query;


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

        private void initSpeakerButton()
        {
            SpeakerButton.Enabled = false;

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
                if (audioEngine.Running == true)
                {
                    StopRecording();
                    SpeakerButton.Highlighted = false;
                }
                else
                {
                    StartRecording();
                    YouAskedLabel.Text = "Listening...";
                    SpeakerButton.Highlighted = true;
                }
            };
        }

        public override void ViewDidLoad()
        {
            vdaGateway = new VDAGateway();
            final_query = "";
            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false; //for iOS5
            
            View.AddGestureRecognizer(g);

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);

            base.ViewDidLoad();

            Querybox.Text = "";
            Querybox.Placeholder = "Your question...";

            YouAskedLabel.TextAlignment = UITextAlignment.Center;
            YouAskedLabel.Text = "";
            YouAskedLabel.LineBreakMode = UILineBreakMode.WordWrap;

            initSpeakerButton();

            HomeSubmitButton.TouchDown += ProcessQuery;

            Querybox.EditingDidEnd += delegate
            {
                final_query = Querybox.Text;
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

            YouAskedLabel.Text = "";
        }

        public async void ProcessQuery(object sender, EventArgs e)
        {
            HomeSubmitButton.Enabled = false;

            var kpiViewController = Storyboard.InstantiateViewController("KpiViewController") as KPIViewController;
            var MySender = sender as KPITableModel;

            //we gotta reset our list variables
            kpiViewController.neededKpi = new List<Kpi>();
            neededKpi = new List<Kpi>();
            
            relatedKpi = new Kpi();

            //grabbing related kpi
            if (string.IsNullOrEmpty(final_query))
            {
                if (!string.IsNullOrEmpty(Querybox.Text))
                {
                    final_query = Querybox.Text;
                }
                else
                {
                    new UIAlertView("Error", "Entry required", null, "OK", null).Show();
                    HomeSubmitButton.Enabled = true;
                    return;
                }
            }

            using (var response = await vdaGateway.RelatedKpi(final_query, dealer_name) as HttpResponseMessage)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        new UIAlertView("Error", "Chat bot service could not identify relevant KPI for your query", null, "OK", null).Show();
                    }
                    else
                    {
                        new UIAlertView("Server Error", "Server status for retrieving your relevant KPI: " + response.StatusCode.ToString(), null, "OK", null).Show();
                    }
                    HomeSubmitButton.Enabled = true;
                    return;
                }

                string json_string = response.Content.ReadAsStringAsync().Result;
                relatedKpi = JsonConvert.DeserializeObject<Kpi>(json_string);

                if (relatedKpi == null)
                {
                    new UIAlertView("Server Error", $"Server returned incompatable model for relvant kpis", null, "OK", null).Show();
                    HomeSubmitButton.Enabled = true;
                    return;
                }

                kpiViewController.relatedKpi = relatedKpi;
            }

            using (var response = await vdaGateway.NeededKpi(dealer_name))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    new UIAlertView("Server Error", "Server status for retrieving your needed KPI: " + response.StatusCode.ToString(), null, "OK", null).Show();
                    HomeSubmitButton.Enabled = true;
                    return;
                }

                string json_string = response.Content.ReadAsStringAsync().Result;
                neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                if (neededKpi == null)
                {
                    new UIAlertView("Server Error", $"Server returned incompatable model for needed kpis", null, "OK", null).Show();
                    HomeSubmitButton.Enabled = true;
                    return;
                }

                kpiViewController.neededKpi = neededKpi;
            }
            this.NavigationController.PushViewController(kpiViewController, true); //This code changes the view     
            HomeSubmitButton.Enabled = true;
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
                        YouAskedLabel.Text = "You asked: " + result.BestTranscription.FormattedString;
                        final_query = result.BestTranscription.FormattedString;
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