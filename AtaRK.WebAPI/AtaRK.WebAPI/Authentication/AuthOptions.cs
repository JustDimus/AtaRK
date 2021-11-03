using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Authentication
{
    public class AuthOptions
    {
        public const string USER_AUTHORIZATION_DATA = "AuthorizationInfo";

        public const string ISSUER = "MyAuthServer";

        public const string AUDIENCE = "MyAuthClient";

        private const string KEY = "50b4ca02b702ffdf63652f3895a0978e0dcf6b1a478934d6d029ee3109334d8f93eac2c290dd31e3c28fcc7d2a10b766336cb347bdeb2833b09957cc5459ae81";
        
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
