using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> AuthorizeAsync(LoginData loginData);

        Task<string> GetLastUsedToken();

        Task<string> UpdateToken();

        IObservable<bool> AuthorizationStatusObserbavle { get; }
    }
}
