using AtaRK.Mobile.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class JsonNetworkRequest : NetworkRequest
    {
        public JsonNetworkRequest(string url, RequestMethod method)
            : base(new Uri(url, UriKind.Relative), method, @"application/json")
        {

        }
    }
}
