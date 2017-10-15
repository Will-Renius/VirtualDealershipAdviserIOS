using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace VDAIOS.Models
{

    public class Kpi
    {
        public string name { get; set; }
        public int value { get; set; }
        public double p_val { get; set; }
        public string segment { get; set; }
        public string brand { get; set; }
    }
}