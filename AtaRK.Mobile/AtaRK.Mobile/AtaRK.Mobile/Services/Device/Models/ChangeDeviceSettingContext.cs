using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Services.Device.Models
{
    public class ChangeDeviceSettingContext
    {
        public string DeviceId { get; set; }

        public string Setting { get; set; }

        public string Value { get; set; }
    }
}
