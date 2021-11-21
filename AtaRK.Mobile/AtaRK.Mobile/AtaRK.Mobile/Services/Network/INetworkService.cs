using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Network
{
    public interface INetworkService
    {
        Task<NetworkResponse> SendRequestAsync(NetworkRequest request);
    }
}
