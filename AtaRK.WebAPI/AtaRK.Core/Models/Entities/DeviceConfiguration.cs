using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class DeviceConfiguration : IBaseEntity
    {
        public Device Device { get; set; }

        public Guid DeviceId { get; set; }

        public Configuration Configuration { get; set; }

        public Guid ConfigurationId { get; set; }
    }
}
