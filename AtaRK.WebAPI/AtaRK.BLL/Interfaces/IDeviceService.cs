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

        Task<ServiceResult<DeviceIdentifier>> InitializeDevice(DeviceInitializationInfo deviceInitialization);

        Task<ServiceResult> UpdateDeviceSettings(DeviceIdentifier deviceInfo, List<DeviceSettingInformation> settings);
    }
}
