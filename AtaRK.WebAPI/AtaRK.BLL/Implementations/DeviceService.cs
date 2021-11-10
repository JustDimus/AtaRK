using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models.Entities;
using AtaRK.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _deviceRepository;

        private readonly IRepository<Configuration> _deviceConfigurationRepository;

        public DeviceService(
            IRepository<Device> deviceRepository,
            IRepository<Configuration> configurationRepository)
        {
            this._deviceConfigurationRepository = configurationRepository ?? throw new ArgumentNullException(nameof(configurationRepository));
            this._deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));

        }

        public Task<ServiceResult> DeleteDeviceAsync(DeviceIdentifier deviceInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<DeviceIdentifier>> InitializeDevice(DeviceInitializationInfo deviceInitialization)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateDeviceAsync(DeviceIdentifier deviceInfo, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateDeviceSettings(DeviceIdentifier deviceInfo, List<DeviceSettingInformation> settings)
        {
            throw new NotImplementedException();
        }
    }
}
