using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class DeviceSetting
    {
        [JsonProperty("setting")]
        public string Setting { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
