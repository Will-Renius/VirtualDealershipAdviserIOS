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

        public Kpi relatedKpi { get; set; } 

        public List<Kpi> neededKpi { get; set; }

        private List<KpiAction> actions;

        public List<Kpi> RNKpi { get; set; } //Related and Needed

        public KPIViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            /* if(relatedKpi.name == "BAD")
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
             }*/

            //RNKpi.Add(relatedKpi);

            List<Kpi> rnkpi = neededKpi;
            rnkpi.Insert(0, relatedKpi);

            UITableView _table;
            _table = new UITableView{
                Frame = new CoreGraphics.CGRect(0,30,View.Bounds.Width,View.Bounds.Height),
                Source = new TableSourceModel(rnkpi)
            };
            View.AddSubview(_table);
            //string a;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            base.PrepareForSegue(segue, sender);

            var actionViewController = segue.DestinationViewController as ActionsViewController;

            //============= Calling our API ======
            actions = new List<KpiAction>();
            try
            {
                string url;
                HttpClient client;
                HttpResponseMessage response;
                string json_string;

                client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Clear();
                //add any default headers below this
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                //grabbing related kpi
                url = $"{BASE_URL}Actions?name={Uri.EscapeDataString("Dealer Sales")}&value={"0.57"}&threshold={"0.5"}";
                response = client.GetAsync(url).Result;
                json_string = response.Content.ReadAsStringAsync().Result;

                actions = JsonConvert.DeserializeObject<List<KpiAction>>(json_string);
                if (relatedKpi != null)
                {
                    actionViewController.actions = actions;
                }

            }
            catch (Exception e)
            {
                new UIAlertView("API Error", "Error with VDA API call: " + e.Message, null, "OK", null).Show();
            }

        }

    }
}