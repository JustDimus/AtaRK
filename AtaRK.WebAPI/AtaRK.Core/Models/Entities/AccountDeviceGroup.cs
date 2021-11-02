using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class AccountDeviceGroup : IBaseEntity
    {
        public Account Account { get; set; }

        public Guid AccountId { get; set; }

        public DeviceGroup Group { get; set; }

        public Guid GroupId { get; set; }

        public MemberRole Role { get; set; }
    }
}
