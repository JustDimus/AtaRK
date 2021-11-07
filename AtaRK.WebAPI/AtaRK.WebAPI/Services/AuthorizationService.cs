using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.Utility.Json;
using AtaRK.WebAPI.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IJsonEncryptionService _encryptionService;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AuthorizationService(
            IJsonEncryptionService encryptionService,
            IHttpContextAccessor contextAccessor)
        {
            this._encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            this._contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public string CreateAccountIdentifier(AuthorizationIdentifier authorizationInfo)
        {
            string encryptedInfo = this._encryptionService.Encrypt(authorizationInfo);

            if (encryptedInfo == null)
            {
                this._logger.Error("Unable to encrypt authorization info");
            }

            return encryptedInfo;
        }

        public AuthorizationIdentifier GetAuthorizedAccountFromCurrentContext()
        {
            var authorizationData = this._contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(i => i.Type == AuthOptions.USER_AUTHORIZATION_DATA);

            if (authorizationData == null)
            {
                this._logger.Error("Unable to extract user authorization data from HTTPContext");
                return null;
            }

            return this.GetAuthorizedAccount(authorizationData.Value);
        }

        public AuthorizationIdentifier GetAuthorizedAccount(string authorizationData)
        {
            var authorizationInfo = this._encryptionService.Decrypt<AuthorizationIdentifier>(authorizationData);

            if (authorizationInfo == null)
            {
                this._logger.Error($"{nameof(authorizationInfo)} is null");
            }

            return authorizationInfo;
        }
    }
}
