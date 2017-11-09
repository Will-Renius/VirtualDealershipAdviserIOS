using Foundation;
using System;
using UIKit;
using Phoneword.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using System.Drawing;
using CoreGraphics;

namespace Phoneword
{
    public partial class LoginViewController : UIViewController
    {
        private List<Kpi> neededKpi;            //List of dealer's worst KPI
        private UIView activeview;             // Controller that activates the keyboard
        private System.nfloat scroll_amount = 0.0f; //Amount of screen that will scroll
        private System.nfloat bottom = 0.0f;    //Button point of the screen

        private float offset = 10.0f;          // Extra offset
        private bool moveViewUp = false;           //Whether the view moves up (depends on keyboard)

        public LoginViewController(IntPtr handle) : base(handle)
        {}


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

            //Maybe remove the username textfields

            //While working in UsernameTextfield, remove keyboard by clicking return
            this.UsernameTextfield.ShouldReturn += (textField) => {
                textField.ResignFirstResponder();
                return true;
            };

            //While working in UsernameTextfield, remove keyboard by clicking return
            this.PasswordTextfield.ShouldReturn += (textField) => {
                textField.ResignFirstResponder();
                return true;
            };

            //Keyboard dismissed on clicking anywhere outside
            var g = new UITapGestureRecognizer(() => View.EndEditing(true));
            g.CancelsTouchesInView = false;
            View.AddGestureRecognizer(g); //Add the tapping gesture to view


            //Moving keyboard up and down
            // Keyboard Goes up
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Dismissed
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardDownNotification);



            neededKpi = new List<Kpi>(); //Used as validation for HTTP request
            string BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/"; 
            //New azure database website, information on each dealer


            string url, json_string, dealer_name, query;

            HttpClient client;
            HttpResponseMessage response = new HttpResponseMessage();

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            MainViewController viewcontroller = Storyboard.InstantiateViewController("ViewController") as MainViewController;
            //Determine next set of view to segue to

            LoginButton.TouchUpInside += (object sender, EventArgs e) =>
            {//Code for transitioning from login view to home view when login button is clicked
               
                if (viewcontroller != null)
                { //If the view controller is null for somereason, does not transfer 

                    dealer_name = UsernameTextfield.Text;
                    url = $"{BASE_URL}NeededKpi?dealer_name={dealer_name}";
                    response = client.GetAsync(url).Result;

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        new UIAlertView("Invalid Credentials", "Invalid Username or Password, please enter valid credentials", null, "OK", null).Show();
                    }
                    else
                    {
                        json_string = response.Content.ReadAsStringAsync().Result;
                        neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                        if (neededKpi == null)
                        {
                            new UIAlertView("Bad Response Recieved", "The Database is currently unavailable, please try again another time", null, "OK", null).Show();
                            //This is never reached because if the needed KPI is null, then response.StatusCode will not equal ok, I presume
                        }

                        else
                        {
                            new UIAlertView("Welcome " + dealer_name," I am your Virtual Dealership Adviser", null, "OK", null).Show();
                            viewcontroller.neededKpi = neededKpi; //Pass needed KPI to the next view (essentially the dealer's data)
                            viewcontroller.dealer_name = dealer_name; //Pass dealer's name to next view

                            this.NavigationController.PushViewController(viewcontroller, true); //This code changes the view
                        }
                    }
                }
            };
        }
    }
}