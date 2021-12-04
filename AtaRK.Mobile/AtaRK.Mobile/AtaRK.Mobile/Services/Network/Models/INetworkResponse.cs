using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network.Models
{
    public interface INetworkResponse
    {
        int ResponseCode { get; }

        string ResponseBody { get; }
    }
}
