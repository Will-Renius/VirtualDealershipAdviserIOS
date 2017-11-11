using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Phoneword.Gateways;
using Phoneword.Models;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Runtime.Serialization;


namespace UnitTests
{
    [TestClass]
    public class VDAGatewayTests
    {
        [TestMethod]
        public async Task EmailPost()
        {
            /*
                public string sender_email { get; set; }
                public string sender_name { get; set; }
                public string receiver_email { get; set; }
                public string receiver_name { get; set; }
                public string message { get; set; }
                public string personal_message { get; set; }
            */

            Email myemail = new Email
            {
                //Will definitely have to change this if this ever gets implemented as a real project
                //  or else Ill be getting emails. Funny thought
                sender_email = "grenfel5@msu.edu",
                sender_name = "Bilbo Baggins",
                receiver_email = "jgrenfell30@gmail.com",
                receiver_name = "Frodo Baggins",
                message = "This is a test message coming at you from Visual Studio! Hello world!",
                personal_message = "And heres a little something personal ;)"
            };

            VDAGateway gateway = new VDAGateway();

            HttpResponseMessage response = await gateway.EmailAction(myemail);

            string errmsg = $"Expecting response [{System.Net.HttpStatusCode.OK}] :: Received [{response.StatusCode}]";
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
