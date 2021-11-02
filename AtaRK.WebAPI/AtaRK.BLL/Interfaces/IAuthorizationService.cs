using AtaRK.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        ServiceResult<Guid> GetAuthorizedAccountId();

        ServiceResult<string> GetAccountIdentifier(Guid accountId);
    }
}
