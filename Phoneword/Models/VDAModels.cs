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
        public string model { get; set; }

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
        public string kpi { get; set; }
        public string type { get; set; }
        public string actionP { get; set; }
        public string actionLink { get; set; }
    }

    public class VerifyLogin
    {
        //will have to change accessor properties later when security matters
        // probably will be weird converting json to model with private properties im guessing?
        public bool validUser { get; set; }
        public bool isAdmin { get; set; }
        public string username { get; set; }
        public string dealer_name { get; set; }

    }

    public class Email
    {
        public string sender_email { get; set; }
        public string sender_name { get; set; }
        public string receiver_email { get; set; }
        public string receiver_name { get; set; }
        public string message { get; set; }
        public string personal_message { get; set; }
    }
}