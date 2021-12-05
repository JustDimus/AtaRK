using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class UpdateDeviceSettingsVM
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("settings")]
        public List<DeviceSettingVM> Settings { get; set; }
    }
}
