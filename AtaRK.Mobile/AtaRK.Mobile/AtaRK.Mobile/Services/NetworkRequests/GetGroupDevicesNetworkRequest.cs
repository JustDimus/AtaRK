using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetGroupDevicesNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetGroupDevicesNetworkRequest(string groupId, string token)
            : base(@"./device/getdevices", Network.Models.RequestMethod.POST, token)
        {
            this.Body = JsonConvert.SerializeObject(new GetGroupDevicesBody()
            {
                GroupId = groupId
            });
        }

        private class GetGroupDevicesBody
        {
            [JsonProperty("body")]
            public string GroupId { get; set; }
        }
    }
}
