using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Authorization.Models
{
    public class AuthorizationModel
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
    }
}
