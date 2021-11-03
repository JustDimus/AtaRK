using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<AuthorizationInfo>> RegisterAsync(AccountRegistrationData registrationData);

        Task<ServiceResult<AuthorizationInfo>> LoginAsync(AccountCredentials credentials);
    }
}
