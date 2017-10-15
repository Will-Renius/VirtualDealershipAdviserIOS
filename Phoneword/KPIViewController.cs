using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

using Phoneword.Models;

namespace Phoneword
{
    public partial class KPIViewController : UIViewController
    {

        public Kpi relatedKpi { get; set; } 

        public List<Kpi> neededKpi { get; set; }


        public KPIViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            if(relatedKpi != null)
            {
                relatedKpiLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                string displayKpi =
                    "Name=" + relatedKpi.name + " :: " +
                    "Value=" + relatedKpi.value + " :: \n" +
                    "Brand=" + relatedKpi.brand + " :: " +
                    "P-Value=" + relatedKpi.p_val;
                relatedKpiLabel.Text = displayKpi;
                relatedKpiLabel.SizeToFit();
            }
            else
            {
                relatedKpiLabel.Text = "No KPI found";
                relatedKpiLabel.SizeToFit();
            }

            string a;
        }
    }
}