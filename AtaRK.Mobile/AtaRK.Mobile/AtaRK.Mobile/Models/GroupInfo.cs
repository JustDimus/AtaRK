using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class GroupInfo
    {
        [JsonProperty("name")]
        public string GroupName { get; set; }

        [JsonProperty("id")]
        public string GroupId { get; set; }
    }
}
