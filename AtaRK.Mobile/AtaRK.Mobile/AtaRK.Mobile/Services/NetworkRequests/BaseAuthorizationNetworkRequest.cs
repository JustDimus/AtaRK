using AtaRK.Mobile.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public abstract class BaseAuthorizationNetworkRequest : JsonNetworkRequest
    {
        public BaseAuthorizationNetworkRequest(string url, RequestMethod method, string token)
            : base(url, method)
        {
            this.Headers.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {token}"));
        }
    }
}
