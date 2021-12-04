using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class FullGroupInfo
    {
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [JsonProperty("user_role")]
        public string UserRole { get; set; }
    }
}
