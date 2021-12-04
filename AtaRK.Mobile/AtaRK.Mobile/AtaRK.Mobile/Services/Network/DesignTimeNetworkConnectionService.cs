using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using System.Text;

namespace AtaRK.Mobile.Services.Network
{
    [ExcludeFromCodeCoverage]
    public class DesignTimeNetworkConnectionService : INetworkConnectionService
    {
        private const bool DEFAULT_NETWORK_CONENCTION_STATUS = true;

        public bool NetworkStatus => DEFAULT_NETWORK_CONENCTION_STATUS;

        private BehaviorSubject<bool> behaviorSubject = new BehaviorSubject<bool>(DEFAULT_NETWORK_CONENCTION_STATUS);

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            return this.behaviorSubject.Subscribe(observer);
        }
    }
}
