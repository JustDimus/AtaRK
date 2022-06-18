using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models
{
    public enum MemberRole : int
    {
        Undefined = 0,
        Owner = 1,
        CoOwner = 2,
        User = 3,
        Spectator = 4
    }
}
