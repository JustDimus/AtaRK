using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class RegisterNetworkRequest : JsonNetworkRequest
    {
        public RegisterNetworkRequest(string email, string password, string firstName, string secondName)
            : base(@"./account/register", Network.Models.RequestMethod.POST)
        {
            this.Body = JsonConvert.SerializeObject(new RegisterBody()
            {
                ConfirmPassword = password,
                SecondName = secondName,
                Email = email,
                FirstName = firstName,
                Password = password
            });
        }

        private class RegisterBody
        {
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }
            [JsonProperty("confirm_password")]
            public string ConfirmPassword { get; set; }
            [JsonProperty("first_name")]
            public string FirstName { get; set; }
            [JsonProperty("second_name")]
            public string SecondName { get; set; }
        }
    }
}
