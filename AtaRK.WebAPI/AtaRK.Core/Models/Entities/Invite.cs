using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class Invite : IBaseEntity
    {
        public Guid CreatorId { get; set; }

        public Account Creator { get; set; }

        public Guid GroupId { get; set; }

        public DeviceGroup Group { get; set; }

        public Guid InvitedId { get; set; }

        public Account Invited { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
