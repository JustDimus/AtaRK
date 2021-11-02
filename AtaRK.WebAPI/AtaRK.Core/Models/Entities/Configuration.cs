using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Core.Models.Entities
{
    public class Configuration : BaseModel
    {
        public DeviceSetting Setting { get; set; }

        public string Value { get; set; }
    }
}
