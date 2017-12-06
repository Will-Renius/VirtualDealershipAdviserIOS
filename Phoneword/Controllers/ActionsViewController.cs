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
        public string url; //Url for taking action
        public List<KpiAction> actions { get; set; } //List of actions
        public string body_message; //Body of email
        public MFMailComposeViewController mailController; //Mail controller

        //Currently selected KpiAction
        public KpiAction selectedAction; 

        //Alert thanking user for taking 
        UIAlertController thankcontroller;

        public ActionsViewController (IntPtr handle) : base (handle)
        {} //Constructor


        public void setSelectedKpi(KpiAction Selected)
        { //Set Action of focus based on which one is clicked
            selectedAction = Selected;

            if (selectedAction.type == "Bad"){ //Message if kpi performing poorly
                body_message = $"Hello, I discovered that our {selectedAction.kpi} is" +
                    " performing poorly. Virtual Dealership Adviser suggests we take the following action:\n\n" +
                    $"{selectedAction.actionP}" + "\n\n" +
                    $"Here is the corresponding action link:\n {selectedAction.actionLink}";

            }

            else{ //Message if kpi performing well
                body_message = $"Hello, I discovered that our {selectedAction.kpi} is" +
                " performing well. Virtual Dealership Adviser suggests we could still make improvements with the following action:\n\n" +
                $"{selectedAction.actionP}" + "\n\n" +
                   $"Here is the corresponding action link:\n {selectedAction.actionLink}";
            }

            //Create thank you alert
            thankcontroller = UIAlertController.Create("Thank you for using Virtual Dealership Adviser", null, UIAlertControllerStyle.Alert);
            thankcontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));

            //Create action alert
            var alertcontroller = UIAlertController.Create("Action transfer", "Here are your options", UIAlertControllerStyle.ActionSheet);

            alertcontroller.AddAction(UIAlertAction.Create("Take Action", UIAlertActionStyle.Default, action => Take_Action()));
            alertcontroller.AddAction(UIAlertAction.Create("Send an Email", UIAlertActionStyle.Default, action => Send_Email()));
            alertcontroller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            PresentViewController(alertcontroller,true,null); //Display action alert

        }

        public void Take_Action(){ //Open assigned url from VirtualDealershipAdviser application
            if (!string.IsNullOrEmpty(url))
            {
                //If no issues open url corresponding to action
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                PresentViewController(thankcontroller, true, null);
            }

            else
            {
                //Error alert
                var unknowncontroller = UIAlertController.Create("Unknown Error", "Please try again another time", UIAlertControllerStyle.Alert);
                unknowncontroller.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
                PresentViewController(unknowncontroller, true, null); //Display action
            }

        }

        public void Send_Email(){ //Open email operation from VirtualDealershipAdviser application
            if (MFMailComposeViewController.CanSendMail)
            {
                //If no issues open email application
                mailController = new MFMailComposeViewController();

                //mailController.SetToRecipients(new string[] { "default email 1,default email 2, etc. });  //Assign default emails
                mailController.SetSubject("Please take a look at this KPI");
                mailController.SetMessageBody(body_message, false);


                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    args.Controller.DismissViewController(true, null);
                };

                PresentViewController(mailController, true, null); //Transition to mail application
                PresentViewController(thankcontroller, true, null); //Display thank you alert
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UITableView _table; 
            _table = new UITableView //Create instance of tableview for the ActionsView
            {
                Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height),
                Source = new ActionTableModel(actions,this)
            };

            //To get rid of table separator lines
            _table.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            //Set background color for the whole table
            _table.BackgroundColor = UIColor.White;

            View.AddSubview(_table); //Add subview to table
        }
    }
}