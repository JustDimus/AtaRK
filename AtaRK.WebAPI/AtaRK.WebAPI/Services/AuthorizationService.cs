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
        private readonly IEncryptionService _encryptionService;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AuthorizationService(
            IEncryptionService encryptionService,
            IHttpContextAccessor contextAccessor)
        {
            this._encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            this._contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public string GetAccountIdentifier(AuthorizationInfo authorizationInfo)
        {
            var convertedInfo = JsonHelper.Serialize(authorizationInfo);

            if (convertedInfo == null)
            {
                this._logger.Error("Unable to convert authorization info");
                return null;
            }

            string encryptedData = this._encryptionService.Encrypt(convertedInfo);

            return encryptedData;
        }

        public AuthorizationInfo GetAuthorizedAccountFromCurrentContext()
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

        public AuthorizationInfo GetAuthorizedAccount(string authorizationData)
        {
            string decryptedData = this._encryptionService.Decrypt(authorizationData);

            if (decryptedData != null)
            {
                var authorizationInfo = JsonHelper.Deserialize<AuthorizationInfo>(decryptedData);

                if (authorizationInfo != null)
                {
                    return authorizationInfo;
                }

                this._logger.Error($"{nameof(authorizationInfo)} is null");
            }
            else
            {
                this._logger.Error($"{nameof(decryptedData)} is null");
            }

            return null;
        }
    }
}
