using Course.MvcApp.Models;
using Course.MvcApp.Models.MvcModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices.Implementation
{
    public class AccountApiService : BaseApiService, IAccountApiService
    {
        public AccountApiService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {

        }

        public Task<AuthorizationModel> LoginAsync(LoginModel loginViewModel)
        {
            return this.SendRequestAsync<AuthorizationModel>("account/login", RequestMethod.POST, loginViewModel);
        }

        public async Task<AuthorizationModel> RegisterAsync(RegistrationModel registrationModel)
        {
            return await this.SendRequestAsync<AuthorizationModel>("account/register", RequestMethod.POST, new
            {
                email = registrationModel.Email,
                password = registrationModel.Password,
                confirm_password = registrationModel.ConfirmPassword,
                first_name = "Martin",
                second_name = "Bob"
            });
        }
    }
}
