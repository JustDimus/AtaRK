using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Interfaces
{
    public interface IDeviceService
    {
        Task<ServiceResult> DeleteDeviceAsync(DeviceIdentifier deviceInfo);

        Task<ServiceResult> UpdateDeviceAsync(DeviceIdentifier deviceInfo, string newName);

        Task<ServiceResult<DeviceIdentifier>> InitializeDevice(DeviceInitializationInfo deviceInitialization, GroupIdentifier groupId, bool isAdminOnly);

        Task<ServiceResult> UpdateDeviceSettings(DeviceIdentifier deviceInfo, List<DeviceSettingInformation> settings, bool isAdminOnly);

        Task<ServiceResult<List<DeviceIdentifier>>> GetDevicesInGroup(GroupIdentifier groupId);

        Task<ServiceResult<List<DeviceSettingInformation>>> GetDeviceSettings(DeviceIdentifier deviceInfo);
    }
}
