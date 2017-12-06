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
        public VerifyLogin login_info;

        private LoadingOverlay loader;

        //Variables for using speech
        private SFSpeechAudioBufferRecognitionRequest recognitionRequest;
        private SFSpeechRecognitionTask recognitionTask;
        private AVAudioEngine audioEngine = new AVAudioEngine();
        private SFSpeechRecognizer speechRecognizer = new SFSpeechRecognizer(new NSLocale("en_US"));

        private Kpi relatedKpi;                 //Kpi most closely matched to dealer question

        public List<Kpi> neededKpi;            //List of dealer's worst KPI
        private UIView activeview;             // Controller that activates the keyboard
        private System.nfloat scroll_amount = 0.0f; //Amount of screen that will scroll
        private System.nfloat bottom = 0.0f;    //Button point of the screen

        private float offset = 10.0f;          // Extra offset
        private bool moveViewUp = false;           //Whether the view moves up (depends on keyboard)

<<<<<<< HEAD
        private VDAGateway vdaGateway;

        string final_query;

<<<<<<< HEAD
        private LoadingOverlay loader;
=======
        NSObject keyboardup;
        NSObject keyboarddown;
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
=======
        private VDAGateway vdaGateway; //Variables for transitioning to different view
        string final_query; //Query passed to Microsoft LUIS
>>>>>>> 0b0da14... Comments Added to iOS


        public MainViewController(IntPtr handle) : base(handle)
        {} //Constructor

        private void KeyBoardUpNotification(NSNotification notification)
        {//Moves the screen up, when the keyboard shows

            //Getting the keyboards size
            var val = (NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameEndUserInfoKey);
            CGRect r = val.CGRectValue;

            // Find what view opened the keyboard and assign it to activeview
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeview = view;
            }

            // Bottom of the controller = initial position + height + offset 
            if (activeview == null)
            {
                bottom = offset;
            }

            else
            {
                bottom = (activeview.Frame.Y + activeview.Frame.Height + offset);
            }

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
            SpeakerButton.Enabled = false; //Click once to start recording, click twice to end recording

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
                    resetTexts();
                    YouAskedLabel.Text = "Listening...";
                    SpeakerButton.Highlighted = true;
                }
            };
        }

        private void resetTexts(){ //Remove user text on delete
            Querybox.Text = "";
            YouAskedLabel.Text = "";
            final_query = "";
            Querybox.Placeholder = "Your question...";
        }

        public override void ViewDidLoad()
        {
            //Welcome alert
            var welcomecontroller = UIAlertController.Create("Welcome", "I am your Virtual Dealership Adviser", UIAlertControllerStyle.Alert);
            welcomecontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            PresentViewController(welcomecontroller, true, null);


            vdaGateway = new VDAGateway();  //Instance of transition between controller views

            resetTexts(); //Text should be empty when first accessing MainViewController

            //Create a tap gesture for movign the keyboard when touching outside of it
            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = true; //for iOS5
            
            View.AddGestureRecognizer(g);

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);


            base.ViewDidLoad();

            DealershipNameLabel.Text = login_info.dealer_name; //Assign attributes of label
            DealershipNameLabel.LineBreakMode = UILineBreakMode.WordWrap;
            

            Querybox.Placeholder = "Your question...";

            YouAskedLabel.TextAlignment = UITextAlignment.Center; //Assign attributes of label
            YouAskedLabel.LineBreakMode = UILineBreakMode.WordWrap;

            initSpeakerButton();

            HomeSubmitButton.TouchDown += ProcessQuery;

            Querybox.ValueChanged += (sender, e) => final_query = Querybox.Text;

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
            resetTexts();
        }



        public async void ProcessQuery(object sender, EventArgs e)
        {
            var bounds = UIScreen.MainScreen.Bounds;

<<<<<<< HEAD
            loader = new LoadingOverlay(bounds);
=======
            loader = new LoadingOverlay(bounds, "Processing query...");
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
            View.Add(loader);

            HomeSubmitButton.Enabled = false;

            var kpiViewController = Storyboard.InstantiateViewController("KpiViewController") as KPIViewController;
            var MySender = sender as KPITableModel;

            //Resets our list variables
            kpiViewController.neededKpi = new List<Kpi>();
            neededKpi = new List<Kpi>();
            
            relatedKpi = new Kpi();

            //Recieve related kpi
            if (string.IsNullOrEmpty(final_query))
            {
                if (!string.IsNullOrEmpty(Querybox.Text))
                {
                    final_query = Querybox.Text;
                }
                else
                {
                    //Error Alert
                    var errorcontroller = UIAlertController.Create("Error", "Entry required", UIAlertControllerStyle.ActionSheet);
                    errorcontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(errorcontroller, true, null); //Display action

                    //new UIAlertView("Error", "Entry required", null, "OK", null).Show();
                    HomeSubmitButton.Enabled = true;
                    loader.Hide();
<<<<<<< HEAD
                    YouAskedLabel = ""
                    final_query = "";
=======
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
                    return;
                }
            }

            using (var response = await vdaGateway.RelatedKpi(final_query, login_info.dealer_name) as HttpResponseMessage)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        //Error Alert
                        var response1controller = UIAlertController.Create("Server Error", "Chat bot service could not identify relevant KPI for your query", UIAlertControllerStyle.ActionSheet);
                        response1controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                        PresentViewController(response1controller, true, null); //Display action

                    }
                    else
                    {
                        //Error Alert
                        var response2controller = UIAlertController.Create("Server Error", "Server status for retrieving your relevant KPI: " + response.StatusCode.ToString(), UIAlertControllerStyle.ActionSheet);
                        response2controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                        PresentViewController(response2controller, true, null); //Display action

                    }
                    HomeSubmitButton.Enabled = true;
<<<<<<< HEAD
                    loader.Hide();
                    final_query = "";
=======
                    resetTexts();
                    loader.Hide();
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
                    return;
                }

                string json_string = response.Content.ReadAsStringAsync().Result;
                relatedKpi = JsonConvert.DeserializeObject<Kpi>(json_string);

                if (relatedKpi == null)
                {
                    //Error Alert
                    var relatedkpicontroller = UIAlertController.Create("Server Error", "Server returned incompatable model for relevant KPIs", UIAlertControllerStyle.ActionSheet);
                    relatedkpicontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(relatedkpicontroller, true, null); //Display action

                    HomeSubmitButton.Enabled = true;
<<<<<<< HEAD
                    loader.Hide();
                    final_query = "";
=======
                    resetTexts();
                    loader.Hide();
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
                    return;
                }

                kpiViewController.relatedKpi = relatedKpi;
            }

            using (var response = await vdaGateway.NeededKpi(login_info.dealer_name))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //Similar to above//////////////////////////////////////////////////////////////////////but want to be cautious of asyncs
                    var response2controller = UIAlertController.Create("Server Error", "Server status for retrieving your relevant KPI: " + response.StatusCode.ToString(), UIAlertControllerStyle.ActionSheet);
                    response2controller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(response2controller, true, null); //Display action

                    HomeSubmitButton.Enabled = true;
<<<<<<< HEAD
                    loader.Hide();
                    final_query = "";
=======
                    resetTexts();
                    loader.Hide();
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
                    return;
                }

                string json_string = response.Content.ReadAsStringAsync().Result;
                neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                if (neededKpi == null)
                {
                    //Similar to above//////////////////////////////////////////////////////////////////////but want to be cautious of asyncs
                    var relatedkpicontroller = UIAlertController.Create("Server Error", "Server returned incompatable model for relevant KPIs", UIAlertControllerStyle.ActionSheet);
                    relatedkpicontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(relatedkpicontroller, true, null); //Display action

                    HomeSubmitButton.Enabled = true;
<<<<<<< HEAD
                    loader.Hide();
                    final_query = "";
=======
                    resetTexts();
                    loader.Hide();
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
                    return;
                }

                kpiViewController.neededKpi = neededKpi;
            }
            this.NavigationController.PushViewController(kpiViewController, true); //This code changes the view     
            HomeSubmitButton.Enabled = true;
<<<<<<< HEAD
            loader.Hide();
            final_query = "";
=======
            resetTexts();
            loader.Hide();
>>>>>>> 5593ef0... Harry: Styling on KPI and Actions page
        }

        // ============== Speech Recognition Functions ============
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