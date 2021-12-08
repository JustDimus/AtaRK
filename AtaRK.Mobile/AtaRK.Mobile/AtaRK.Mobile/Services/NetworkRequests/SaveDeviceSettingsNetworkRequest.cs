using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class SaveDeviceSettingsNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public SaveDeviceSettingsNetworkRequest(string deviceId, string setting, string value, string token)
            : base (@"device/update", Network.Models.RequestMethod.POST, token)
        {
            this.Body = JsonConvert.SerializeObject(new SaveDeviceSettingsBody()
            {
                DeviceId = deviceId,
                Settings = new List<DeviceSettingBody>
                {
                    new DeviceSettingBody()
                    {
                        Setting = setting,
                        Value = value
                    }
                }
            });
        }

        private class SaveDeviceSettingsBody
        {
            [JsonProperty("device_id")]
            public string DeviceId { get; set; }

            [JsonProperty("settings")]
            public List<DeviceSettingBody> Settings { get; set; }
        }

        private class DeviceSettingBody
        {
            [JsonProperty("setting")]
            public string Setting { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
