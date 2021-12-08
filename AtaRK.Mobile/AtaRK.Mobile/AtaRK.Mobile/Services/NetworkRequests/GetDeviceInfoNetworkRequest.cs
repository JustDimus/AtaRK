using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetDeviceInfoNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetDeviceInfoNetworkRequest(string deviceId, string token)
            : base(@"device/info", Network.Models.RequestMethod.POST, token)
        {
            this.Body = JsonConvert.SerializeObject(new GetDeviceInfoBody()
            {
                DeviceId = deviceId
            });
        }

        private class GetDeviceInfoBody
        {
            [JsonProperty("body")]
            public string DeviceId { get; set; }
        }
    }
}
