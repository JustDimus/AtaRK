using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetGroupInformationNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetGroupInformationNetworkRequest(string groupId, string token)
            : base(@"group/groupinfo", Network.Models.RequestMethod.POST, token)
        {
            this.Body = JsonConvert.SerializeObject(new GetGroupInformationBody()
            {
                GroupId = groupId
            });
        }

        private class GetGroupInformationBody
        {
            [JsonProperty("body")]
            public string GroupId { get; set; }
        }
    }
}
