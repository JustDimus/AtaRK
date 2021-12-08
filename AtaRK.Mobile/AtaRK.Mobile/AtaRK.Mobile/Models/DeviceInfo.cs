using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class DeviceInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string DeviceName { get; set; }
    }
}
