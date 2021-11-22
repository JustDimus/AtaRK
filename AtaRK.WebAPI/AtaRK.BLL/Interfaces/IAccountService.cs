using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<AuthorizationIdentifier>> RegisterAsync(AccountRegistrationData registrationData);

        Task<ServiceResult<AuthorizationIdentifier>> LoginAsync(AccountCredentials credentials);

        Task<ServiceResult> ChangePasswordAsync(PasswordChange passwordChange);

        Task<ServiceResult> ChangeAccountAsync(AccountInformation accountInfo);

        Task<ServiceResult<AuthorizationIdentifier>> GetAccountByEmailAsync(string email);

        Task<ServiceResult<AccountInformation>> GetAuthorizedAccountInformationAsync();
    }
}
