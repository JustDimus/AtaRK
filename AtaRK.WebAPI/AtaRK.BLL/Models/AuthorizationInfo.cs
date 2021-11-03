using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Models
{
    public class AuthorizationInfo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
