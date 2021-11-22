using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class Account : BaseModel
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public List<Invite> CreatedInvitations { get; set; }

        public List<Invite> Invitations { get; set; }

        public List<AccountDeviceGroup> Groups { get; set; }
    }
}
