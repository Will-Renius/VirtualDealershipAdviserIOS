using System;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Drawing;
//using System.Drawing.RectangleF;
using VDAIOS.Models;

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Session; //session state
//using Microsoft.Extensions.Caching.Distributed;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
//speech to text
using System.Net;
using System.IO;
using System.Threading;

namespace Phoneword
{
    public partial class ViewController : UIViewController
    {
        string test_string;
        string user_inp;

        private UIView activeview;             // Controller that activated the keyboard
        private float scroll_amount = 0.0f;    // amount to scroll 
        private float bottom = 0.0f;           // bottom point
        private float offset = 10.0f;          // extra offset
        private bool moveViewUp = false;           // which direction are we moving

        //UIScrollView scrollView;
        //UIImageView imageView;


        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

           // HomeSubmitButton.TouchUpInside += HomeSubmitButton_TouchUpInside;


            /*
            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyBoardDownNotification);
            */
            //scrollView = new UIScrollView(new CoreGraphics.CGRect(0, 0, View.Frame.Width, View.Frame.Height));
            //View.AddSubview(scrollView);
            // Perform any additional setup after loading the view, typically from a nib.
        }

        /*
        private void KeyBoardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            RectangleF r = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
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
        {
            if (moveViewUp) { ScrollTheView(false); }
        }

        private void ScrollTheView(bool move)
        {

            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            RectangleF frame = View.Frame;

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
        */

        /*private void HomeSubmitButton_TouchUpInside(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            test_string = "THIS IS A TEST!";
            //user_inp = string.Parse (HomeTextField.Text);
            user_inp = HomeTextField.Text;
        }

        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var kpiViewController = segue.DestinationViewController as KPIViewController;
            kpiViewController.user_inp_KPI = this.user_inp;
        }*/
        

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void HomeDeleteButton_TouchUpInside(UIButton sender)
        {
            QuestionLabel.Text = string.Format("");
        }

        partial void HomeSubmitButton_TouchUpInside(UIButton sender)
        {
            //Sets info in the text view into an objrct of class
            QuestionData.info = HomeTextField.Text;

           /* string related_kpi_url = "http://virtualdealershipadvisorapi.azurewebsites.net/api/RelatedKpi";
            try
            {
                string url = related_kpi_url + "?query=" + Uri.EscapeDataString(QuestionData.info);
                var client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                //add any default headers below this
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url);
                string json_string = await response.Content.ReadAsStringAsync();

                Kpi most_related_kpi = JsonConvert.DeserializeObject<Kpi>(json_string);
                ViewBag.most_related_kpi = most_related_kpi;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }*/

        }
    }
}