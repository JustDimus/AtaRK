using AtaRK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        AuthorizationInfo GetAuthorizedAccountFromCurrentContext();

        AuthorizationInfo GetAuthorizedAccount(string authorizationData);

        string GetAccountIdentifier(AuthorizationInfo authorizationInfo);
    }
}
