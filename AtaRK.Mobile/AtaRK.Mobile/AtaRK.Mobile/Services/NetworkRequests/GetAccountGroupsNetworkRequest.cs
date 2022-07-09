using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class GetAccountGroupsNetworkRequest : BaseAuthorizationNetworkRequest
    {
        public GetAccountGroupsNetworkRequest(string token)
            : base(@"group/list", Network.Models.RequestMethod.GET, token)
        {

        }
    }
}
