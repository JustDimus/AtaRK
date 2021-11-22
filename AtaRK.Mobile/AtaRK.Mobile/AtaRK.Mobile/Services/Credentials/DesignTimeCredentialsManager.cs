using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Credentials
{
    public class DesignTimeCredentialsManager : ICredentialsManager
    {
        private LoginCredentials loginCredentials;

        public bool ContainsCredentials => this.loginCredentials != null;

        public Task<bool> EraseCredentials()
        {
            this.loginCredentials = null;

            return Task.FromResult(true);
        }

        public Task<LoginCredentials> GetCredentialsAsync()
        {
            return Task.FromResult(this.loginCredentials);
        }

        public Task<bool> SaveCredentialsAsync(LoginCredentials credentials)
        {
            this.loginCredentials = credentials;

            return Task.FromResult(true);
        }
    }
}
