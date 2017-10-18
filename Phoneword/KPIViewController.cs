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
//using Phoneword.Models.TableSourceModel;

namespace Phoneword
{
    public partial class KPIViewController : UIViewController
    {
        //The members passed into View from Home page
        public Kpi relatedKpi { get; set; }
        public List<Kpi> neededKpi { get; set; }

        //currently selected
        private Kpi selectedKpi;

        public void setSelectedKpi(Kpi Selected)
        {
            selectedKpi = Selected;
        }

        //This will be set when we hit the API
        private List<KpiAction> actions;
        //Container to store our KPIs in the list view
        private List<Kpi> RNKpi { get; set; } //Related and Needed
        private UITableView _table;

        public KPIViewController(IntPtr handle) : base(handle)
        {
            selectedKpi = new Kpi();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            List<Kpi> rnkpi = new List<Kpi>();

            //eventually we should disable the actions button when this is here.
            if (neededKpi != null && relatedKpi != null)
            {
                rnkpi = neededKpi;
                rnkpi.Insert(0, relatedKpi);
            }
            else
            {
                new UIAlertView("NULL Reference", "Null reference from previous window!", null, "OK", null).Show();
                rnkpi.Add(new Kpi { name = "Null reference" });
            }

            _table = new UITableView
            {
                Frame = new CoreGraphics.CGRect(0, 100, View.Bounds.Width, View.Bounds.Height),
                Source = new TableSourceModel(rnkpi, this)
            };
            View.AddSubview(_table);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            base.PrepareForSegue(segue, sender);

            var actionViewController = segue.DestinationViewController as ActionsViewController;

            //gotta reset our list variables
            actionViewController.actions = new List<KpiAction>();
            actions = new List<KpiAction>();

            //============= Calling our API ======
            actions = new List<KpiAction>();

            string url, json_string, name;
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
                new UIAlertView("Selection Error", "No KPI selected, defaulting to related KPI...", null, "OK", null).Show();
                name = relatedKpi.name;
            }
            else
            {
                name = selectedKpi.name;
            }

            url = $"{BASE_URL}Actions?name={Uri.EscapeDataString(name)}&value={relatedKpi.p_val}&threshold={"0.5"}";

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
                new UIAlertView("API Error", $"Status Code: {response.StatusCode.ToString()}", null, "OK", null).Show();
                actions.Add(new KpiAction { actionP = "Please select another KPI" });
            }
            else //proceed as normal otherwise
            {
                json_string = response.Content.ReadAsStringAsync().Result;
                actions = JsonConvert.DeserializeObject<List<KpiAction>>(json_string);
                if (actions == null)
                {
                    new UIAlertView("Deserialization ERR", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                    actions.Add(new KpiAction { actionP = "Please select another KPI" });
                }
            }
            actionViewController.actions = actions;

        }

    }
}