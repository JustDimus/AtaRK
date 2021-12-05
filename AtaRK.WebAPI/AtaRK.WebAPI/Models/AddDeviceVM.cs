using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class AddDeviceVM
    {
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("device_code")]
        public string DeviceCode { get; set; }
    }
}
