using Foundation;
using System;
using UIKit;

using Phoneword.Models;
using System.Collections.Generic;

namespace Phoneword
{
    public partial class ActionsViewController : UIViewController
    {

        public List<KpiAction> actions { get; set; }
        public ActionsViewController (IntPtr handle) : base (handle)
        {

        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            new UIAlertView(
                "Temp",
                "Processed...",
                null,
                "OK",
                null
                );
        }
    }
}