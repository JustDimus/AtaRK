using AtaRK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        AuthorizationIdentifier GetAuthorizedAccountFromCurrentContext();

        AuthorizationIdentifier GetAuthorizedAccount(string authorizationData);

        string CreateAccountIdentifier(AuthorizationIdentifier authorizationInfo);
    }
}
