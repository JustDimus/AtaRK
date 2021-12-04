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

        [JsonProperty("type")]
        public string DeviceType { get; set; }

        [JsonProperty("code")]
        public string DeviceCode { get; set; }
    }
}
