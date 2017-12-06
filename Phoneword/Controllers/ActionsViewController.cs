using Foundation;
using System;
using UIKit;

using Phoneword.Models;
using System.Collections.Generic;
using MessageUI;

using Phoneword.Gateways;
using System.Net.Http;

namespace Phoneword
{
    public partial class ActionsViewController : UIViewController
    {
        public string url;
        public List<KpiAction> actions { get; set; }
        public string body_message;
        public MFMailComposeViewController mailController;

        //currently selected KpiAction
        public KpiAction selectedAction; 

        //alert thanking user for taking 
        UIAlertController thankcontroller;

        public ActionsViewController (IntPtr handle) : base (handle)
        {

        }



        /*public ActionsViewController(IntPtr handle) : base(handle)
        {
            //Initialize Object
            selectedAction = new KpiAction();
        }*/

        public void setSelectedKpi(KpiAction Selected)
        {
            selectedAction = Selected;

            if (selectedAction.type == "Bad"){ //Action performing poorly
                body_message = $"Hello, I discovered that our {selectedAction.kpi} is" +
                    " performing poorly. Virtual Dealership Adviser suggests we take the following action:\n\n" +
                    $"{selectedAction.actionP}" + "\n\n" +
                    $"Here is the corresponding action link:\n {selectedAction.actionLink}";

            }

            else{ //Action performing well
                body_message = $"Hello, I discovered that our {selectedAction.kpi} is" +
                " performing well. Virtual Dealership Adviser suggests we could still make improvements with the following action:\n\n" +
                $"{selectedAction.actionP}" + "\n\n" +
                   $"Here is the corresponding action link:\n {selectedAction.actionLink}";
            }

            thankcontroller = UIAlertController.Create("Thank you for using Virtual Dealership Adviser", null, UIAlertControllerStyle.Alert);
            thankcontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));

            //Create alert of action selection with three buttons
            var alertcontroller = UIAlertController.Create("Action transfer", "Here are your options", UIAlertControllerStyle.ActionSheet);

            alertcontroller.AddAction(UIAlertAction.Create("Take Action", UIAlertActionStyle.Default, action => Take_Action()));
            alertcontroller.AddAction(UIAlertAction.Create("Send an Email", UIAlertActionStyle.Default, action => Send_Email()));
            alertcontroller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            PresentViewController(alertcontroller,true,null); //Display action

        }

        public void Take_Action(){
            if (!string.IsNullOrEmpty(url))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                PresentViewController(thankcontroller, true, null);
            }

            else
            {
                var unknowncontroller = UIAlertController.Create("Unknown Error", "Please try again another time", UIAlertControllerStyle.Alert);
                PresentViewController(unknowncontroller, true, null); //Display action
                //var alertcontroller = UIAlertController.Create("Action transfer", "Here are your options", UIAlertControllerStyle.ActionSheet);
                //Error Message not needed?
            }

        }

        public void Send_Email(){

            /*
            if (action_selected == 0)
            {
                //Error Message not needed?
                return;
            }
            */


            /*
            //What I was testing the EmailAction function with
            Email myemail = new Email
            {
                //Will definitely have to change this if this ever gets implemented as a real project
                //  or else Ill be getting emails. Funny thought
                sender_email = "grenfel5@msu.edu",
                sender_name = "Bilbo Baggins",
                receiver_email = "jgrenfell30@gmail.com",
                receiver_name = "Frodo Baggins",
                message = "This is a test message coming at you from Visual Studio! Hello world! " + selectedAction.kpi + selectedAction.actionLink,
                personal_message = "And heres a little something personal ;)"
            };

            VDAGateway gateway = new VDAGateway();

            HttpResponseMessage response = await gateway.EmailAction(myemail);

            return;
            */

            if (MFMailComposeViewController.CanSendMail)
            {

                mailController = new MFMailComposeViewController();

                mailController.SetToRecipients(new string[] { "reniuswi@msu.edu" }); //, "kobinaoforidankwa@gmail.com" });
                mailController.SetSubject("Please take a look at this KPI");
                mailController.SetMessageBody(body_message, false);


                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    //Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };

                PresentViewController(mailController, true, null); //

                PresentViewController(thankcontroller, true, null); //Display action
            }
        }

        public override void ViewDidLoad()
        {
            //MFMailComposeViewController mailController;

            //MFMailComposeViewController mailController = new MFMailComposeViewController();

            base.ViewDidLoad();

            UITableView _table;
            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height),
                Source = new ActionTableModel(actions,this)
            };

            //To get rid of table seperator lines
            _table.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            //Set background color for the whole table
            _table.BackgroundColor = UIColor.White;

            View.AddSubview(_table);
        }
    }
}