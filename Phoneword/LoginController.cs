using Foundation;
using System;
using UIKit;
using Phoneword.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Phoneword
{
    public partial class LoginController : UIViewController
    {
        private List<Kpi> neededKpi;

        public LoginController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            neededKpi = new List<Kpi>();
            //string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            string BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/"; //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<New Post
            //Team database containing info on each dealer

            string url, json_string, dealer_name, query;

            HttpClient client;
            HttpResponseMessage response = new HttpResponseMessage();

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            //add any default headers below this
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));



            //var viewController = segue.DestinationViewController as ViewController;

            ViewController viewcontroller = Storyboard.InstantiateViewController("ViewController") as ViewController;
            LoginButton.TouchUpInside += (object sender, EventArgs e) =>
            {//code for transitioning from login view to home view when login button is clicked
               

                if (viewcontroller != null)
                {
                    dealer_name = UsernameTextfield.Text;
                    url = $"{BASE_URL}NeededKpi?dealer_name={dealer_name}";
                    response = client.GetAsync(url).Result;

                    //json_string = response.Content.ReadAsStringAsync().Result;
                    //neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        new UIAlertView("GET /NeededKpi ERR:", response.StatusCode.ToString(), null, "OK", null).Show();

                        neededKpi.Add(new Kpi { name = "Error finding your needed KPIs" });
                    }
                    else
                    {
                        json_string = response.Content.ReadAsStringAsync().Result;
                        neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                        if (neededKpi == null)
                        {
                            new UIAlertView("Deserialization ERR", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                            neededKpi.Add(new Kpi { name = "Error deserializing your needed kpis" });
                        }

                        else
                        {
                            new UIAlertView("Welcome: " + dealer_name + " I am ViDA your Virtual Dealership Adviser", null, null, "OK", null).Show();
                            viewcontroller.neededKpi = neededKpi; //Pass needed KPI to the next view (essentially the dealer's data
                            viewcontroller.dealer_name = dealer_name;
                            //PasswordTextfield.Text = neededKpi[0].name + "," + neededKpi[0].p_val.ToString();

                            this.NavigationController.PushViewController(viewcontroller, true);
                        }
                    }




                }
            };
        }
    }
}