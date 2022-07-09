using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network.Models
{
    public abstract class NetworkRequest : INetworkRequest
    {
        public NetworkRequest(Uri url, RequestMethod requestMethod, string contentType)
        {
            this.Url = url;
            this.Method = requestMethod;
            this.Headers = new List<KeyValuePair<string, string>>();
            this.MediaType = contentType;
        }

        public Uri Url { get; }

        public List<KeyValuePair<string, string>> Headers { get; }

        public RequestMethod Method { get; }

        public string Body { get; protected set; }

        public string MediaType { get; }
    }
}
