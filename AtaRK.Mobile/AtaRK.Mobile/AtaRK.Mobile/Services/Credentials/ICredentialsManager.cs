using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Credentials
{
    public interface ICredentialsManager
    {
        bool ContainsCredentials { get; }

        Task<bool> EraseCredentials();

        Task<LoginCredentials> GetCredentialsAsync();
        
        Task<bool> SaveCredentialsAsync(LoginCredentials credentials);
    }
}
