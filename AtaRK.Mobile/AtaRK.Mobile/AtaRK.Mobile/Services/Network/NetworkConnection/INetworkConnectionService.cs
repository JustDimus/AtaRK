using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network.NetworkConnection
{
    public interface INetworkConnectionService : IObservable<bool>
    {
        bool NetworkStatus { get; }
    }
}
