using System;

using UIKit;

namespace Phoneword
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /*partial void HomeSubmitButton_TouchUpInside(UIButton sender)
        {
            //throw new NotImplementedException();
            QuestionLabel.Text = string.Format("You Clicked The Submit Button!");
        }*/

        partial void HomeDeleteButton_TouchUpInside(UIButton sender)
        {
            QuestionLabel.Text = string.Format("");
        }


    }
}