using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models;
using AtaRK.Core.Models.Entities;
using AtaRK.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _deviceRepository;

        private readonly IRepository<Configuration> _deviceConfigurationRepository;

        private readonly IRepository<AccountDeviceGroup> _accountGroupRepository;

        private readonly IAuthorizationService _authorizationService;

        public DeviceService(
            IRepository<Device> deviceRepository,
            IRepository<AccountDeviceGroup> accountGroupRepository,
            IRepository<Configuration> configurationRepository,
            IAuthorizationService authorizationService)
        {
            this._deviceConfigurationRepository = configurationRepository ?? throw new ArgumentNullException(nameof(configurationRepository));
            this._deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            this._authorizationService = authorizationService;
            this._accountGroupRepository = accountGroupRepository;
        }

        public async Task<ServiceResult> DeleteDeviceAsync(DeviceIdentifier deviceInfo)
        {
            if (!this.GetCurrentAccount(out var account))
            {
                return false;
            }

            try
            {
                var device = await this._deviceRepository.FirstOrDefaultAsync(i => i.Id == deviceInfo.Id);

                if (device == null)
                {
                    return false;
                }

                var accountGroupInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.GroupId == device.GroupId && i.AccountId == account.Id);

                if (accountGroupInfo == null)
                {
                    return false;
                }

                if (device.AdminOnlyAccess
                    && accountGroupInfo.Role != MemberRole.Owner
                    && accountGroupInfo.Role != MemberRole.CoOwner)
                {
                    return false;
                }

                await this._deviceRepository.DeleteAsync(device);

                await this._deviceRepository.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ServiceResult<List<DeviceSettingInformation>>> GetDeviceSettings(DeviceIdentifier deviceInfo)
        {
            if (!this.GetCurrentAccount(out var account))
            {
                return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
            }

            try
            {
                var device = await this._deviceRepository.FirstOrDefaultAsync(i => i.Id == deviceInfo.Id);

                if (device == null)
                {
                    return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
                }

                var accountGroupInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.GroupId == device.GroupId && i.AccountId == account.Id);

                if (accountGroupInfo == null)
                {
                    return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
                }


                var settings = (await this._deviceConfigurationRepository.SelectAsync(
                    i => i.DeviceId == deviceInfo.Id,
                    i => new DeviceSettingInformation()
                    {
                        Name = i.Setting,
                        Value = i.Value
                    })).ToList();

                return ServiceResult<List<DeviceSettingInformation>>.FromResult(settings);
            }
            catch
            {
                return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
            }
        }

        public async Task<ServiceResult<List<DeviceIdentifier>>> GetDevicesInGroup(GroupIdentifier groupId)
        {
            if (!this.GetCurrentAccount(out var account))
            {
                return ServiceResult<List<DeviceIdentifier>>.Instance(false);
            }

            try
            {
                var devices = await this._deviceRepository.SelectAsync(i => i.GroupId == groupId.Id);

                var accountGroupInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.GroupId == groupId.Id && i.AccountId == account.Id);

                if (accountGroupInfo == null)
                {
                    return ServiceResult<List<DeviceIdentifier>>.Instance(false);
                }

                return devices.Select(i => new DeviceIdentifier()
                {
                    Id = i.Id,
                    Name = i.DeviceName
                }).ToList();
            }
            catch
            {
                return ServiceResult<List<DeviceIdentifier>>.Instance(false);
            }
        }

        public async Task<ServiceResult<DeviceIdentifier>> InitializeDevice(DeviceInitializationInfo deviceInitialization, GroupIdentifier groupId, bool isAdminOnly)
        {
            if (!this.GetCurrentAccount(out var account))
            {
                return ServiceResult<DeviceIdentifier>.Instance(false);
            }

            try
            {
                var accountGroup = await this._accountGroupRepository.FirstOrDefaultAsync(i => i.AccountId == account.Id && i.GroupId == groupId.Id);

                if (accountGroup == null || accountGroup.Role == MemberRole.Spectator)
                {
                    return ServiceResult<DeviceIdentifier>.Instance(false);
                }

                var isDeviceExist = await this._deviceRepository
                    .AnyAsync(i => i.GroupId == groupId.Id && i.DeviceName == deviceInitialization.UniqueId);

                if (isDeviceExist)
                {
                    return ServiceResult<DeviceIdentifier>.Instance(false); 
                }

                if (isAdminOnly
                    && accountGroup.Role != MemberRole.Owner
                    && accountGroup.Role != MemberRole.CoOwner)
                {
                    return ServiceResult<DeviceIdentifier>.Instance(false);
                }

                Device newDevice = new Device()
                {
                    GroupId = groupId.Id,
                    AdminOnlyAccess = isAdminOnly,
                    DeviceName = deviceInitialization.UniqueId,
                    Type = DeviceType.SARA3
                };

                await this._deviceRepository.CreateAsync(newDevice);

                await this._deviceRepository.SaveAsync();

                return ServiceResult<DeviceIdentifier>.FromResult(new DeviceIdentifier()
                {
                    Id = newDevice.Id,
                    Name = newDevice.DeviceName
                });
            }
            catch
            {
                return ServiceResult<DeviceIdentifier>.Instance(false);
            }
        }

        public Task<ServiceResult> UpdateDeviceAsync(DeviceIdentifier deviceInfo, string newName)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateDeviceSettings(DeviceIdentifier deviceInfo, List<DeviceSettingInformation> settings, bool isAdminOnly)
        {
            if (!this.GetCurrentAccount(out var account))
            {
                return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
            }

            try
            {
                var device = await this._deviceRepository.FirstOrDefaultAsync(i => i.Id == deviceInfo.Id);

                if (device == null)
                {
                    return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
                }

                var accountGroupInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.GroupId == device.GroupId && i.AccountId == account.Id);

                if (accountGroupInfo == null)
                {
                    return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
                }

                if (device.AdminOnlyAccess
                    && accountGroupInfo.Role != MemberRole.Owner
                    && accountGroupInfo.Role != MemberRole.CoOwner)
                {
                    return false;
                }

                if (isAdminOnly
                    && accountGroupInfo.Role != MemberRole.Owner
                    && accountGroupInfo.Role != MemberRole.CoOwner)
                {
                    return false;
                }

                device.AdminOnlyAccess = isAdminOnly;

                var deviceSettings = (await this._deviceConfigurationRepository.SelectAsync(
                    i => i.DeviceId == deviceInfo.Id)).ToList();

                foreach (var i in settings)
                {
                    var setting = deviceSettings.FirstOrDefault(k => i.Name == k.Setting);

                    if (setting != null)
                    {
                        setting.Value = i.Value;
                    }
                    else
                    {
                        deviceSettings.Add(new Configuration()
                        {
                            DeviceId = device.Id,
                            Setting = i.Name,
                            Value = i.Value
                        });
                    }
                }

                await this._deviceRepository.UpdateAsync(device);

                foreach (var i in deviceSettings)
                {
                    await this._deviceConfigurationRepository.UpdateAsync(i);
                }

                await this._deviceRepository.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                return ServiceResult<List<DeviceSettingInformation>>.Instance(false);
            }
        }

        private bool GetCurrentAccount(out AuthorizationIdentifier authorizationIdentifier)
        {
            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            authorizationIdentifier = account;

            return account != null;
        }
    }
}
