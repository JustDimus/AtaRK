using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class CreateNewDeviceNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public CreateNewDeviceNetworkRequest(string groupId, string deviceType, string deviceCode, string token)
             : base(@"device/add", Network.Models.RequestMethod.POST, token)
        {
            this.Body = JsonConvert.SerializeObject(new CreateNewDeviceBody()
            {
                DeviceCode = deviceCode,
                DeviceType = deviceType,
                GroupId = groupId
            });
        }

        private class CreateNewDeviceBody
        {
            [JsonProperty("group_id")]
            public string GroupId { get; set; }

            [JsonProperty("device_type")]
            public string DeviceType { get; set; }

            [JsonProperty("device_code")]
            public string DeviceCode { get; set; }
        }
    }
}
