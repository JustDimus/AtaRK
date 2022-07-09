using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Device.Models;
using AtaRK.Mobile.Services.Network.Models;
using AtaRK.Mobile.Services.Network.NetworkConnection;
using AtaRK.Mobile.Services.Network.Service;
using AtaRK.Mobile.Services.NetworkRequests;
using AtaRK.Mobile.Services.Serializer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DataManager
{
    public class NetworkDataManager : IDataManager, IDisposable
    {
        private static int SUCCESS_RESPONSE_CODE = 200;

        private INetworkConnectionService _connectionService;
        private IAuthorizationService _authorizationService;
        private INetworkService _networkService;
        private ISerializer _serializer;

        private bool lastAuthorizationStatus;
        private bool lastNetworkConnectionStatus;

        private IDisposable authorizationChangedDisposable;
        private IDisposable networkConnectionChangedDisposable;

        public NetworkDataManager(
            INetworkService networkService,
            INetworkConnectionService networkConnectionService,
            IAuthorizationService authorizationService,
            ISerializer serializer)
        {
            this._serializer = serializer;
            this._networkService = networkService;
            this._authorizationService = authorizationService;
            this._connectionService = networkConnectionService;

            this.authorizationChangedDisposable = this._authorizationService.AuthorizationStatusObserbavle.Subscribe(this.OnAuthorizationStatusChanged);
            this.networkConnectionChangedDisposable = this._connectionService.Subscribe(this.OnNetworkConnectionChanged);
        }

        private bool CanSendRequest => this.lastNetworkConnectionStatus;

        public async Task<bool> CreateNewDevice(CreateNewDeviceContext settingContext)
        {
            return await this.SendRequestWithAuthorizationWithoutResponse(token => new CreateNewDeviceNetworkRequest(
                settingContext.GroupId,
                settingContext.DeviceType,
                settingContext.DeviceCode,
                token));
        }

        public async Task<RequestContext<FullGroupInfo>> GetGroupInfo(string groupId)
        {
            return await this.SendRequestWithAuthorization<FullGroupInfo>(token => new GetGroupInformationNetworkRequest(groupId, token));
        }

        public async Task<RequestContext<ListData<DeviceSetting>>> GetDeviceSettings(string deviceId)
        {
            return await this.SendRequestWithAuthorization<ListData<DeviceSetting>>(token => new GetDeviceSettingsNetworkRequest(deviceId, token));
        }

        public async Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId)
        {
            return await this.SendRequestWithAuthorization<ListData<DeviceInfo>>(token => new GetGroupDevicesNetworkRequest(groupId, token));
        }

        public async Task<RequestContext<ListData<GroupInfo>>> GetGroupsInfo()
        {
            return await this.SendRequestWithAuthorization<ListData<GroupInfo>>(token => new GetAccountGroupsNetworkRequest(token));
        }

        public async Task<RequestContext<DeviceInfo>> GetDeviceInfo(string deviceId)
        {
            return await this.SendRequestWithAuthorization<DeviceInfo>(token => new GetDeviceInfoNetworkRequest(deviceId, token));
        }

        public async Task<bool> SaveDeviceSetting(ChangeDeviceSettingContext settingContext)
        {
            return await this.SendRequestWithAuthorizationWithoutResponse(token => new SaveDeviceSettingsNetworkRequest(
                settingContext.DeviceId,
                settingContext.Setting,
                settingContext.Value,
                token));
        }

        private void OnNetworkConnectionChanged(bool connectionStatus)
        {
            this.lastNetworkConnectionStatus = connectionStatus;
        }

        private void OnAuthorizationStatusChanged(bool authorizationStatus)
        {
            this.lastAuthorizationStatus = authorizationStatus;
        }

        private async Task<RequestContext<TResult>> SendRequestWithAuthorization<TResult>(Func<string, INetworkRequest> requestConstructor) where TResult : class
        {
            if (!CanSendRequest)
            {
                return RequestContext.FromStatus<TResult>(false);
            }

            var authorizationToken = await this._authorizationService.UpdateToken();

            if (!string.IsNullOrEmpty(authorizationToken))
            {
                var request = requestConstructor?.Invoke(authorizationToken);

                var result = await this._networkService.SendRequestAsync(request);

                if (result.ResponseCode == SUCCESS_RESPONSE_CODE)
                {
                    var serializedData = this._serializer.Deserialize<TResult>(result.ResponseBody);

                    if (serializedData != null)
                    {
                        return new RequestContext<TResult>()
                        {
                            IsSuccessful = true,
                            Result = serializedData
                        };
                    }
                }
            }

            return RequestContext.FromStatus<TResult>(false);
        }

        private async Task<RequestContext> SendRequestWithAuthorizationWithoutResponse(Func<string, INetworkRequest> requestConstructor)
        {
            if (!CanSendRequest)
            {
                return new RequestContext()
                {
                    IsSuccessful = false
                };
            }

            var authorizationToken = await this._authorizationService.UpdateToken();

            if (this.lastAuthorizationStatus)
            {
                var request = requestConstructor?.Invoke(authorizationToken);

                var result = await this._networkService.SendRequestAsync(request);

                if (result.ResponseCode == SUCCESS_RESPONSE_CODE)
                {
                    return new RequestContext()
                    {
                        IsSuccessful = true
                    };
                }
            }

            return new RequestContext()
            {
                IsSuccessful = false
            };
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.authorizationChangedDisposable?.Dispose();
                    this.networkConnectionChangedDisposable?.Dispose();
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
