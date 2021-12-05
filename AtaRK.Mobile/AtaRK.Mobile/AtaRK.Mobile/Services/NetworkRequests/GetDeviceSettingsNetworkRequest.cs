using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetDeviceSettingsNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetDeviceSettingsNetworkRequest(string deviceId, string authorizationToken)
            : base(@"./device/info", Network.Models.RequestMethod.POST, authorizationToken)
        {
            this.Body = JsonConvert.SerializeObject(new GetDeviceSettingsBody()
            {
                DeviceId = deviceId
            });
        }

        private class GetDeviceSettingsBody
        {
            [JsonProperty("body")]
            public string DeviceId { get; set; }
        }
    }
}
