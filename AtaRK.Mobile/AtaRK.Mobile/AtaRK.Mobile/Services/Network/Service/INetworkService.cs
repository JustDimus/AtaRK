using AtaRK.Mobile.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Network.Service
{
    public interface INetworkService : IDisposable
    {
        Task<INetworkResponse> SendRequestAsync(INetworkRequest request);
    }
}
