using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.Device.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DataManager
{
    public interface IDataManager
    {
        Task<RequestContext<ListData<GroupInfo>>> GetGroupsInfo();

        Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId);

        Task<RequestContext<ListData<DeviceSetting>>> GetDeviceSettings(string deviceId);

        Task<RequestContext<DeviceInfo>> GetDeviceInfo(string deviceId);

        Task<bool> SaveDeviceSetting(ChangeDeviceSettingContext settingContext);
    }
}
