using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Device.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Device
{
    public interface IDeviceService
    {
        IObservable<DeviceInfo> DeviceInfoObservable { get; }

        IObservable<ChangeDeviceSettingContext> DeviceSettingObservable { get; }

        Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId);

        Task<bool> GetDeviceInfo(string deviceId);

        Task<RequestContext<ListData<DeviceSetting>>> GetDeviceSettings(string deviceId);

        void SetCurrentSettingChangeContext(ChangeDeviceSettingContext settingContext);

        Task<bool> SaveDeviceSettingContext(ChangeDeviceSettingContext settingContext);
    }
}
