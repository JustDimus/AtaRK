using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class DeviceInfo
    {
        [JsonProperty("device_id")]
        public string Id { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("device_code")]
        public string DeviceCode { get; set; }
    }
}
