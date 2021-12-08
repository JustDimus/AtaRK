using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class LoginNetworkRequest : JsonNetworkRequest
    {
        public LoginNetworkRequest(string email, string password)
            : base(@"account/login", Network.Models.RequestMethod.POST)
        {
            this.Body = JsonConvert.SerializeObject(new LoginBody()
            {
                Email = email,
                Password = password
            });
        }

        private class LoginBody
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}
