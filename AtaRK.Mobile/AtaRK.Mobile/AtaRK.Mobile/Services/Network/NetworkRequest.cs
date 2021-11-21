using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network
{
    public enum RequestType
    {
        GET,
        POST
    }

    public class NetworkRequest
    {
        Uri RequestUrl { get; set; }

        RequestType Type { get; set; }

        string RequestBody { get; set; }
    }
}
