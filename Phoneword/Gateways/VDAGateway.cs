using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Phoneword.Models;

using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Phoneword.Gateways
{
    public class VDAGateway
    {
        private static readonly HttpClient VDAClient;
        public static readonly string BASE_URL;
        static VDAGateway()
        {
            VDAClient = new HttpClient();
            VDAClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            BASE_URL = "http://msufall2017virtualdealershipadviserapi.azurewebsites.net/api";
        }

        public async Task<HttpResponseMessage> VerifyLogin(string username, string password)
        {
            HttpResponseMessage response = await VDAGetRequest($"/VerifyLogin?username={username}&password={password}");
            return response;
        }

        public async Task<HttpResponseMessage> Actions(string name, double value, double threshold = 0.5)
        {
            HttpResponseMessage response = await VDAGetRequest($"/Actions?name={name}&value={value}&threshold={threshold}");
            return response;
        }

        public async Task<HttpResponseMessage> RelatedKpi(string query, string dealer_name)
        {
            HttpResponseMessage response = await VDAGetRequest($"/RelatedKpi?query={query}&dealer_name={dealer_name}"); ;
            return response;
        }

        public async Task<HttpResponseMessage> NeededKpi(string dealer_name)
        {
            HttpResponseMessage response = await VDAGetRequest($"/NeededKpi?dealer_name={dealer_name}");
            return response;
        }

        public async Task<HttpResponseMessage> EmailAction(Email email)
        {
            string email_json = JsonConvert.SerializeObject(email);
            HttpContent content = new StringContent(email_json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await VDAPostRequest("/Email", content);
            return response;
        }

        private async Task<HttpResponseMessage> VDAGetRequest(string endpoint)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string url = BASE_URL + endpoint;

            //give'er 3 tries!
            for (int i = 0; i < 3; i++)
            {
                response = await VDAClient.GetAsync(url);
                if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    //some other stuff?
                    break;
                }
                else
                {
                    //some other stuff
                    continue;
                }
            }
            return response;
        }

        private async Task<HttpResponseMessage> VDAPostRequest(string endpoint, HttpContent content)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string url = BASE_URL + endpoint;

            //give'er 3 tries!
            for (int i = 0; i < 3; i++)
            {
                response = await VDAClient.PostAsync(url, content);
                if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    //some other stuff?
                    break;
                }
                else
                {
                    //some other stuff
                    continue;
                }
            }
            return response;
        }
    }
}