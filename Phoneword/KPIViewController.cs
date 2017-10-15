using Foundation;
using System;
using UIKit;
using VDAIOS.Models;


namespace Phoneword
{
    public partial class KPIViewController : UIViewController
    {

        string test_string_KPI;
        public string user_inp_KPI;


        public KPIViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            KPITextView.Text = QuestionData.info;
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void KPITestButton_TouchUpInside(UIButton sender)
        {
            //throw new NotImplementedException();
            KPITextView.Text = string.Format("You clicked the button");
        }

        partial void KPIActionsButton_TouchUpInside(UIButton sender)
        {
            //Takes from text view that is in the KPI view controller, and sets it to an object
            //Will need to change it to take info from the api 
            QuestionData.info = KPITextView.Text;
        }
    }
}