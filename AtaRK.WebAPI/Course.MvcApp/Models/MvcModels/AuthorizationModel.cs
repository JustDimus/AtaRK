using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class AuthorizationModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
