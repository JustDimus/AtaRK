using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class DeviceGroup : BaseModel
    {
        public string GroupName { get; set; }

        public List<AccountDeviceGroup> Members { get; set; }
        
        public List<Device> Devices { get; set; }

        public List<Invite> Invitations { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
