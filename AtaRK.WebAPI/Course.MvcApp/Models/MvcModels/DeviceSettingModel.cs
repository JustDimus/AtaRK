using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class DeviceSettingModel
    {
        [JsonProperty("setting")]
        public string Setting { get; set; }
        
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
