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
        Task<ServiceResult<string>> RegisterAsync(AccountRegistrationData registrationData);

        Task<ServiceResult<string>> LoginAsync(AccountCredentials credentials);
    }
}
