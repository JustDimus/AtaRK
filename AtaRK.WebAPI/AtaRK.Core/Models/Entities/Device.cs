using AtaRK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class Device : BaseModel
    {
        public string DeviceName { get; set; }

        public DeviceType Type { get; set; }

        public DeviceGroup Group { get; set; }

        public Guid GroupId { get; set; }

        public List<Configuration> Configurations { get; set; }
    }
}
