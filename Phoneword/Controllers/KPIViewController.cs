using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

using Phoneword.Models;
using Phoneword.Gateways;

//API client functions 
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Phoneword
{
    public partial class KPIViewController : UIViewController
    {
        //The members passed into View from Home page
        public Kpi relatedKpi { get; set; }
        public List<Kpi> neededKpi { get; set; }

        //This will be set when we hit the API
        private List<KpiAction> actions;

        //Container to store our KPIs in the list view
        private List<Kpi> RNKpi { get; set; } //Related and Needed kpis combined

        private UITableView _table; //Table like view for this controller
        private KPITableModel TableSource; //Assist in table view

        private VDAGateway VDAGateway; //For transitioning between views

        public KPIViewController(IntPtr handle) : base(handle)
        {} //Constructor

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            List<Kpi> rnkpi = new List<Kpi>();
            VDAGateway = new VDAGateway();

            if (neededKpi != null && relatedKpi != null)
            {
                rnkpi = neededKpi; //Sets list to all the needkPIs
            }

            //Initialize table view and its source
            TableSource = new KPITableModel(rnkpi,relatedKpi); //Passing in needed list and related kpi seperately
            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height),
                Source = TableSource
            };

            //To get rid of table seperator lines
            _table.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            //Set background color for the whole table
            _table.BackgroundColor = UIColor.White;
            View.AddSubview(_table);

            TableSource.NewPageEvent += KpiSelected;
        }

        public async void KpiSelected(object sender, EventArgs e)
        {
            
            _table.UserInteractionEnabled = false;

            //Transitioning to ActionsViewController
            ActionsViewController nextPage = this.Storyboard.InstantiateViewController("ActionsViewController") as ActionsViewController;
            KPITableModel MySender = sender as KPITableModel; 

            Kpi selectedKpi = MySender.getSelected();

            if (nextPage != null)
            {
                //gotta reset our list variables
                actions = new List<KpiAction>();

                //============= Calling our API ======
                string json_string; // for future use if we wanna define a threshold

                if (string.IsNullOrEmpty(selectedKpi.name))
                {
                    //Error Alert
                    var selectioncontroller = UIAlertController.Create("Selection Error", "Please select a KPI", UIAlertControllerStyle.Alert);
                    selectioncontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(selectioncontroller, true, null);

                    //new UIAlertView("Selection Error", "Please select a KPI", null, "OK", null).Show();
                    return;
                }

                var response = await VDAGateway.Actions(selectedKpi.name, selectedKpi.p_val);

                //if we still have internal server error
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //Error Alert
                    var apicontroller = UIAlertController.Create("API Error", "Server under maintenance, please try again later", UIAlertControllerStyle.Alert);
                    apicontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(apicontroller, true, null);

                    //new UIAlertView("API Error", $"Server under maintenance, please try again later", null, "OK", null).Show();
                    return;
                }

                json_string = response.Content.ReadAsStringAsync().Result;
                actions = JsonConvert.DeserializeObject<List<KpiAction>>(json_string);

                if (actions == null)
                {
                    //Error Alert
                    var deserializationcontroller = UIAlertController.Create("Deserialization Error",$"JSON Returned: \"{json_string}\"", UIAlertControllerStyle.Alert);
                    deserializationcontroller.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(deserializationcontroller, true, null);

                    //new UIAlertView("Deserialization Error", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                    return;
                }

                nextPage.actions = new List<KpiAction>();
                nextPage.actions = actions;

                this.NavigationController.PushViewController(nextPage, true); //This code changes the view
            }

            _table.UserInteractionEnabled = true;
        }
    }

}