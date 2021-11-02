using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Models.DTO
{
    public class AccountRegistrationData
    {
        public AccountCredentials Credentials { get; set; }

        public AccountInformation Information { get; set; }
    }
}
