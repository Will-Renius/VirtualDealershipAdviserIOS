using Foundation;
using System;
using UIKit;

using Phoneword.Models;

namespace Phoneword
{
    public partial class KPIViewController : UIViewController
    {

        public Kpi relatedKpi { get; set; } 


        public KPIViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            if(relatedKpi != null)
            {
                string displayKpi =
                    "Name=" + relatedKpi.name + " :: " +
                    "Value=" + relatedKpi.value + " :: " +
                    "Brand=" + relatedKpi.brand + " :: " +
                    "P-Value=" + relatedKpi.p_val;
                relatedKpiLabel.Text = displayKpi;
                relatedKpiLabel.SizeToFit();
            }
            else
            {
                relatedKpiLabel.Text = "You suck.";
                relatedKpiLabel.SizeToFit();
            }
        }
    }
}