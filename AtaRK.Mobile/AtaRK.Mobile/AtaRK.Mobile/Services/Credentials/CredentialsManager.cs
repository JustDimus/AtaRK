using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Credentials
{
    public class CredentialsManager : ICredentialsManager
    {
        public CredentialsManager()
        {

        }

        public bool ContainsCredentials => throw new NotImplementedException();

        public Task<bool> EraseCredentials()
        {
            throw new NotImplementedException();
        }

        public Task<LoginCredentials> GetCredentialsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveCredentialsAsync(LoginCredentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
