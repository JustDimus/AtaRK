using Course.MvcApp.Models;
using Course.MvcApp.Models.MvcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices
{
    public interface IAccountApiService
    {
        Task<AuthorizationModel> LoginAsync(LoginModel loginViewModel);

        Task<AuthorizationModel> RegisterAsync(RegistrationModel registrationModel);
    }
}
