using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class DeviceShowModel
    {
        [JsonProperty("Name")]
        public string DeviceName { get; set; }

        [JsonProperty("Id")]
        public string DeviceId { get; set; }

        [JsonProperty("list")]
        public IEnumerable<DeviceSettingModel> Settings { get; set; }
    }
}
