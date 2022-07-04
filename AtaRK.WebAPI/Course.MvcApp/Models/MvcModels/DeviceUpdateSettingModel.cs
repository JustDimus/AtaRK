using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class DeviceUpdateSettingModel
    {
        public string DeviceId { get; set; }

        public string Setting { get; set; }

        public string Value { get; set; }

        public bool IsAdminOnly { get; set; }

        public bool IsReadonly { get; set; }
    }
}
