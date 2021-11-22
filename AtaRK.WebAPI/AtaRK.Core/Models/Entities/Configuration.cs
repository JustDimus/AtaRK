using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class Configuration : BaseModel
    {
        public string Setting { get; set; }

        public string Value { get; set; }

        public Device Device { get; set; }

        public Guid DeviceId { get; set; }
    }
}
