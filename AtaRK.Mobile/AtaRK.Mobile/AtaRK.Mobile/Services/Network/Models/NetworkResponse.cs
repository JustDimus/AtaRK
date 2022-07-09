using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network.Models
{
    public class NetworkResponse : INetworkResponse
    {
        public int ResponseCode { get; set; }

        public string ResponseBody { get; set; }
    }
}
