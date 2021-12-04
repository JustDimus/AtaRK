using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Group.UserRole
{
    public class UserRoleManager
    {
        public UserRole GetUserRole(string userRole)
        {
            if (!string.IsNullOrEmpty(userRole) && Enum.TryParse<UserRole>(userRole, out var result))
            {
                return result;
            }

            return UserRole.Undefined;
        }
    }
}
