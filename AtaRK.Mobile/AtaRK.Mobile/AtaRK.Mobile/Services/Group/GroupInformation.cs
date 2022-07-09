using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Group
{
    public class GroupInformation
    {
        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public UserRole.UserRole UserRole { get; set; }
    }
}
