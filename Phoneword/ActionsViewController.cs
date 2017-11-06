using Foundation;
using System;
using UIKit;

using Phoneword.Models;
using System.Collections.Generic;
using MessageUI;

namespace Phoneword
{
    public partial class ActionsViewController : UIViewController
    {

        public List<KpiAction> actions { get; set; }
        public ActionsViewController (IntPtr handle) : base (handle)
        {

        }

        //currently selected
        private KpiAction selectedAction;

        /*public ActionsViewController(IntPtr handle) : base(handle)
        {
            //Initialize Object
            selectedAction = new KpiAction();
        }*/

        public void setSelectedKpi(KpiAction Selected)
        {
            selectedAction = Selected;
        }

        public override void ViewDidLoad()
        {
            //
            MFMailComposeViewController mailController;
            //MFMailComposeViewController mailController = new MFMailComposeViewController();

            //Take a screen shot
            UIGraphics.BeginImageContext(View.Frame.Size);
           View.DrawViewHierarchy(View.Frame, true);
           UIImage image = UIGraphics.GetImageFromCurrentImageContext();
           UIGraphics.EndImageContext();

            base.ViewDidLoad();

            UITableView _table;
            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 100, View.Bounds.Width, View.Bounds.Height),
                Source = new TableActionModel(actions,this)
            };
            View.AddSubview(_table);

            TakeActionButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.urbanscience.com/"));
                //Opens 
            };

            EmailButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (MFMailComposeViewController.CanSendMail)
                {
                    mailController = new MFMailComposeViewController();

                    mailController.SetToRecipients(new string[] { "kobinaoforidankwa@gmail.com" });
                    mailController.SetSubject("Test subject");
                    mailController.SetMessageBody("This should be a couple of paragraphs",false);

                    mailController.AddAttachmentData(image.AsPNG(), "image/png", "Screenshot.png");

                    mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                        Console.WriteLine(args.Result.ToString());
                        args.Controller.DismissViewController(true, null);
                    };

                    this.PresentViewController(mailController, true, null);

                    new UIAlertView("Thank you for using Virtual Dealership Adviser", " An email has been sent to your desired recipient", null, "OK", null);

                }
            };
            // Perform any additional setup after loading the view, typically from a nib.
            /* new UIAlertView(
                 "Temp",
                 "Processed...",
                 null,
                 "OK",
                 null
                 );*/
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
           // string filler = "filler";
        }
    }
}