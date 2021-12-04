using AtaRK.Mobile.Services.Authorization.Models;
using AtaRK.Mobile.Services.Network;
using AtaRK.Mobile.Services.NetworkRequests;
using AtaRK.Mobile.Services.Serializer;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly INetworkService _networkService;
        private readonly ISerializer _serializer;

        private ReplaySubject<bool> authorizationStatusSubject = new ReplaySubject<bool>(1);

        private string lastUsedToken;
        private LoginData lastLoginData;

        public AuthorizationService(
            INetworkService networkService,
            ISerializer serializer)
        {
            this._networkService = networkService ?? throw new ArgumentNullException(nameof(networkService));
            this._serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public IObservable<bool> AuthorizationStatusObserbavle => authorizationStatusSubject.AsObservable();

        public async Task<string> GetToken()
        {
            if (this.lastLoginData == null)
            {
                return null;
            }

            var loginResult = await this.LoginAsync(this.lastLoginData);

            return this.lastUsedToken;
        }

        public async Task<bool> LoginAsync(LoginData loginData)
        {
            if (loginData == null)
            {
                return false;
            }

            var request = new LoginNetworkRequest(loginData.Email, loginData.Password);

            var result = await this._networkService.SendRequestAsync(request);

            if (result.ResponseCode == 200)
            {
                var token = this._serializer.Deserialize<AuthorizationModel>(result.ResponseBody);

                this.lastUsedToken = token.Token;
                this.lastLoginData = loginData;
            }

            return false;
        }
    }
}
