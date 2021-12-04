using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<bool> LoginAsync(LoginData loginData);

        IObservable<bool> AuthorizationStatusObserbavle { get; }
    }
}
