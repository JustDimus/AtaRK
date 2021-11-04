using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class PasswordChange
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
