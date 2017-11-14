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

using Phoneword.Gateways;
using System.Threading.Tasks;

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

        private VDAGateway vdaGateway;

        LoadingOverlay loader;


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
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Dismissed
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);

            vdaGateway = new VDAGateway();

            LoginButton.TouchDown += LoginRequested;

            
        }

        public async void LoginRequested(object sender, EventArgs e)
        {
            var bounds = UIScreen.MainScreen.Bounds;

            loader = new LoadingOverlay(bounds);
            View.Add(loader);


            LoginButton.Enabled = false;


            MainViewController viewcontroller = Storyboard.InstantiateViewController("MainViewController") as MainViewController;
            KPITableModel MySender = sender as KPITableModel;

            if (viewcontroller != null)
            { //If the view controller is null for somereason, does not transfer 

                string username = UsernameTextfield.Text;
                string password = PasswordTextfield.Text;

                Task<HttpResponseMessage> TaskResponse = vdaGateway.VerifyLogin(username, password);

                var response = await TaskResponse as HttpResponseMessage;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    new UIAlertView("Invalid Credentials", "Invalid Username or Password, please enter valid credentials", null, "OK", null).Show();
                }
                else
                {
                    string json_string = await response.Content.ReadAsStringAsync();
                    var verifiedLogin = JsonConvert.DeserializeObject<VerifyLogin>(json_string) as VerifyLogin;

                    if (verifiedLogin == null)
                    {
                        new UIAlertView("Server Error", "Server response unable to be read", null, "OK", null).Show();
                    }

                    else
                    {
                        //new UIAlertView("Welcome " + dealer_name," I am your Virtual Dealership Adviser", null, "OK", null).Show();
                        viewcontroller.login_info = verifiedLogin; //Pass dealer's name to next view
                        
                        this.NavigationController.PushViewController(viewcontroller, true); //This code changes the view
                    }
                }
            }
            LoginButton.Enabled = true;
            loader.Hide();

        }
    }
}