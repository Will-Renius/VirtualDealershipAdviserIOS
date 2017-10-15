using Foundation;
using System;
using UIKit;



//Copied from UrbanScience Capstone WebApp
/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session; //session state
using Microsoft.Extensions.Caching.Distributed;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
//speech to text
using System.Net;
using System.IO;
using System.Threading;*/

namespace Phoneword
{
    //View Controller for displaying actions
    public partial class ActionsViewController : UIViewController
    {
        public ActionsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Sets info in the object class to be displayed by the view
            ActionTextView.Text = QuestionData.info;

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.


        }
    }
}