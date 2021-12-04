using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network.Models
{
    public enum RequestMethod
    {
        GET = 0,
        POST = 1
    }

    public interface INetworkRequest
    {
        Uri Url { get; }

        List<KeyValuePair<string, string>> Headers { get; }

        RequestMethod Method { get; }

        string Body { get; }

        string MediaType { get; }
    }
}
