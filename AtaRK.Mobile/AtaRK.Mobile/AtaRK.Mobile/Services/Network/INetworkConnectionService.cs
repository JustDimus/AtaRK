using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Network
{
    public interface INetworkConnectionService : IObservable<bool>
    {
        bool NetworkStatus { get; }
    }
}
