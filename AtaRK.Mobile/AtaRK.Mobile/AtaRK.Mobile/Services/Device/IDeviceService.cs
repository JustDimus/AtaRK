using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Device
{
    public interface IDeviceService
    {
        IObservable<DeviceInfo> DeviceInfoObservable { get; }

        Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId);

        Task<bool> GetDeviceInfo(string deviceId);
    }
}
