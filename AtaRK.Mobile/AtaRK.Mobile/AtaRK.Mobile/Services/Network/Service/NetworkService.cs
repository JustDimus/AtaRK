using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Network.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Network.Service
{
    public class NetworkService : INetworkService
    {
        private HttpClient client = null;

        public NetworkService(
            NetworkSettings settings)
        {
            this.client = new HttpClient();

            if (settings.UseRelativeUrls)
            {
                this.client.BaseAddress = new Uri(settings.BaseURL);
            }
        }

        public Task<INetworkResponse> SendRequestAsync(INetworkRequest request)
        {
            return Task.Run(async () => await this.SendRequest(request));
        }

        private async Task<INetworkResponse> SendRequest(INetworkRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (var httpRequest = new HttpRequestMessage()
            {
                Method = this.GetHttpMethod(request.Method),
                Content = request.Body == null ? null : new StringContent(request.Body, Encoding.UTF8, request.MediaType),
                RequestUri = request.Url
            })
            {
                request.Headers?.ForEach(i => httpRequest.Headers.Add(i.Key, i.Value));

                return await GetResponse(httpRequest);
            }
        }

        private async Task<INetworkResponse> GetResponse(HttpRequestMessage message)
        {
            INetworkResponse result = null;

            using (CancellationTokenSource source = new CancellationTokenSource(100000))
            {
                try
                {
                    var response = await this.client.SendAsync(message, source.Token);

                    result = new NetworkResponse()
                    {
                        ResponseCode = ((int)response.StatusCode),
                        ResponseBody = await response.Content.ReadAsStringAsync()
                    };
                }
                catch (Exception ex)
                {
                    result = new NetworkResponse()
                    {
                        ResponseCode = -1
                    };
                }
            }

            return result;
        }

        private HttpMethod GetHttpMethod(RequestMethod method)
        {
            switch (method)
            {
                case RequestMethod.GET:
                    return HttpMethod.Get;
                case RequestMethod.POST:
                    return HttpMethod.Post;
                default:
                    throw new ArgumentException(nameof(method));
            }
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //Managed resources
                    this.client?.Dispose();
                }

                //Unmanaged resources

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
