using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Xamarin.Essentials;

namespace AtaRK.Mobile.Services.Network.NetworkConnection
{
    public class NetworkConnectionService : INetworkConnectionService
    {
        private bool lastNetworkStatus = false;

        private ReplaySubject<bool> replaySubject;

        public bool NetworkStatus => this.lastNetworkStatus;

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            if (replaySubject == null)
            {
                this.replaySubject = new ReplaySubject<bool>(1);

                Connectivity.ConnectivityChanged += (sender, e) => this.OnConnectivityChanged(e.NetworkAccess);

                this.OnConnectivityChanged(Connectivity.NetworkAccess);
            }

            return replaySubject.Subscribe(observer);
        }

        public void OnConnectivityChanged(NetworkAccess networkState)
        {
            var hasNetworkConnection = networkState >= NetworkAccess.ConstrainedInternet;

            this.lastNetworkStatus = hasNetworkConnection;

            this.replaySubject.OnNext(hasNetworkConnection);
        }
    }
}
