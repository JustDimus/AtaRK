using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetInfoNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetInfoNetworkRequest(string token)
            : base(@"account/info", Network.Models.RequestMethod.GET, token)
        {

        }
    }
}
