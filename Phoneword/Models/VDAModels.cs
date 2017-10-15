using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Phoneword.Models
{
    public class Kpi
    {
        public int id;
        public string name { get; set; }
        public int value { get; set; }
        public double p_val { get; set; }
        public string segment { get; set; }
        public string brand { get; set; }
        public string dealer { get; set; }
        public string month { get; set; }
        public string type { get; set; }
        public List<KpiAction> action_list { get; set; } 
    }

    public class KpiAction
    {
        //i was thinking of this as whether it's good or bad, not sure what to label it
        public string response { get; set; }
        public string text { get; set; }
    }
}