using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

using Phoneword.Models;

//API client functions 
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;


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
        private List<Kpi> RNKpi { get; set; } //Related and Needed
        private UITableView _table;
        private KPITableModel TableSource;

        public KPIViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            List<Kpi> rnkpi = new List<Kpi>();

            if (neededKpi != null && relatedKpi != null)
            {
                rnkpi = neededKpi;
                rnkpi.Insert(0, relatedKpi);
            }

            TableSource = new KPITableModel(rnkpi);
            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 100, View.Bounds.Width, View.Bounds.Height),
                Source = TableSource
            };

            //To get rid of table seperator lines
            _table.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            //Set background color for the whole table
            _table.BackgroundColor = UIColor.FromRGB(204, 255, 153);
            View.AddSubview(_table);

            
            TableSource.NewPageEvent += KpiSelected;
        }

        public void KpiSelected(object sender, EventArgs e)
        {

            ActionsViewController nextPage = this.Storyboard.InstantiateViewController("ActionsViewController") as ActionsViewController;
            KPITableModel MySender = sender as KPITableModel;
            Kpi selectedKpi = MySender.getSelected();

            string BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/"; //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< New Post

            if (nextPage != null)
            {
                //gotta reset our list variables
                actions = new List<KpiAction>();

                //============= Calling our API ======
                string url, json_string,
                    threshold = ""; // for future use if we wanna define a threshold
                HttpClient client;
                HttpResponseMessage response = new HttpResponseMessage();

                client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                //add any default headers below this
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                //grabbing related kpi
                //checking if I selected any kpi
                if (string.IsNullOrEmpty(selectedKpi.name))
                {
                    new UIAlertView("Selection Error", "Please select a KPI", null, "OK", null).Show();
                    return;
                }

                url = $"{BASE_URL}Actions?name={Uri.EscapeDataString(selectedKpi.name)}&value={selectedKpi.p_val}";
                if(!string.IsNullOrEmpty(threshold))
                {
                    url = url + $"&threshold={threshold}";
                }

                //give'er 3 tries!
                for (int i = 0; i < 3; i++)
                {
                    response = client.GetAsync(url).Result;
                    if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                    {
                        //some other stuff?
                        break;
                    }
                    else
                    {
                        //BUG: name = Dealer Share doesnt work in api
                        //some stuff probably
                        continue;
                    }
                }

                //if we still have internal server error
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    new UIAlertView("API Error", $"Server under maintenance, please try again later", null, "OK", null).Show();
                    return;
                }

                json_string = response.Content.ReadAsStringAsync().Result;
                actions = JsonConvert.DeserializeObject<List<KpiAction>>(json_string);
                if (actions == null)
                {
                    new UIAlertView("Deserialization Error", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                    return;
                }


                nextPage.actions = new List<KpiAction>();

                nextPage.actions = actions;


                this.NavigationController.PushViewController(nextPage, true); //This code changes the view

            }
        }
    }

}
 