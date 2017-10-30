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
            neededKpi = new List<Kpi>(); //Used as validation for HTTP request
            //string BASE_URL = "http://virtualdealershipadvisorapi.azurewebsites.net/api/";
            string BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api/"; 
            //New azure database website, information on each dealer


            string url, json_string, dealer_name, query;

            HttpClient client;
            HttpResponseMessage response = new HttpResponseMessage();

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            ViewController viewcontroller = Storyboard.InstantiateViewController("ViewController") as ViewController;
            //Determine next set of view to segue to

            LoginButton.TouchUpInside += (object sender, EventArgs e) =>
            {//Code for transitioning from login view to home view when login button is clicked
               

                if (viewcontroller != null)
                { //If the view controller is null for somereason, does not transfer 

                    dealer_name = UsernameTextfield.Text;
                    url = $"{BASE_URL}NeededKpi?dealer_name={dealer_name}";
                    response = client.GetAsync(url).Result;

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //new UIAlertView("GET /NeededKpi ERR:", response.StatusCode.ToString(), null, "OK", null).Show();
                        new UIAlertView("Bad Response Recieved", "The Database is currently unavailable, please try again another time", null, "OK", null).Show();
                        //Error code message
                    }
                    else
                    {
                        json_string = response.Content.ReadAsStringAsync().Result;
                        neededKpi = JsonConvert.DeserializeObject<List<Kpi>>(json_string);

                        if (neededKpi == null)
                        {
                            //new UIAlertView("Deserialization ERR", $"JSON Returned: \"{json_string}\"", null, "OK", null).Show();
                            new UIAlertView("Invalid Credentials","Invalid Username or Password, please enter valid credentials",null,"OK",null).Show();
                            //Invalid credentials error message

                            //This is never reached because if the needed KPI is null, then response.StatusCode will not equal ok, I presume
                        }

                        else
                        {
                            new UIAlertView("Welcome" + dealer_name," I am your Virtual Dealership Adviser, ready to help", null, "OK", null).Show();
                            viewcontroller.neededKpi = neededKpi; //Pass needed KPI to the next view (essentially the dealer's data)
                            viewcontroller.dealer_name = dealer_name; //Pass dealer's name to next view

                            this.NavigationController.PushViewController(viewcontroller, true);
                        }
                    }
                }
            };
        }
    }
}