using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetInfoNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetInfoNetworkRequest(string token)
            : base(new Uri(@"./account/info", UriKind.Relative), Network.Models.RequestMethod.GET, token)
        {

        }
    }
}
