using AtaRK.Mobile.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.NetworkRequests
{
    public class JsonNetworkRequest : NetworkRequest
    {
        public JsonNetworkRequest(Uri url, RequestMethod method)
            : base(url, method, @"application/json")
        {

        }
    }
}
