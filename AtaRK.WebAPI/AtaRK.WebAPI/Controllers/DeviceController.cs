using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IJsonEncryptionService _encryptionService;

        public DeviceController(
            IDeviceService deviceService,
            IJsonEncryptionService encryptionService)
        {
            this._deviceService = deviceService;
            this._encryptionService = encryptionService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> ListDevices([FromBody] SingleBodyParameter groupInfo)
        {
            var groupId = this.DeserializeGroupInfo(groupInfo.Body);

            var serviceResult = await this._deviceService.GetDevicesInGroup(groupId);

            if (serviceResult)
            {
                return new JsonResult(new { list = serviceResult.Result.Select(i => new
                {
                    Name = i.Name,
                    Id = this.SerializeDeviceInfo(i)
                })});
            }

            return Conflict();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDevice([FromBody] AddDeviceVM deviceInfo)
        {
            var groupInfo = this.DeserializeGroupInfo(deviceInfo.GroupId);

            var deviceInitializationInfo = new DeviceInitializationInfo()
            {
                DeviceType = deviceInfo.DeviceType,
                UniqueId = deviceInfo.DeviceCode,
                IsAdminOnlyDevice = deviceInfo.IsAdminOnly
            };

            var serviceResult = await this._deviceService.InitializeDevice(deviceInitializationInfo, groupInfo, deviceInfo.IsAdminOnly);

            if (serviceResult)
            {
                return new JsonResult(new { DeviceId = this.SerializeDeviceInfo(serviceResult.Result)});
            }

            return Conflict();
        }

        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> RemoveDevice([FromBody] SingleBodyParameter deviceInfo)
        {
            var deviceId = this.DeserializeDeviceInfo(deviceInfo.Body);

            var serviceResult = await this._deviceService.DeleteDeviceAsync(deviceId);

            if (serviceResult)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateDeviceSettingsVM updateInfo)
        {
            var deviceId = this.DeserializeDeviceInfo(updateInfo.DeviceId);

            var settings = updateInfo.Settings.Select(i => new DeviceSettingInformation()
            {
                Name = i.Setting,
                Value = i.Value
            }).ToList();

            var serviceResult = await this._deviceService.UpdateDeviceSettings(deviceId, settings, updateInfo.IsAdminOnly);

            if (serviceResult)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> GetDeviceSettings([FromBody] SingleBodyParameter deviceInfo)
        {
            var deviceId = this.DeserializeDeviceInfo(deviceInfo.Body);

            var serviceResult = await this._deviceService.GetDeviceSettings(deviceId);

            if (serviceResult)
            {
                return new JsonResult(new
                {
                    Name = deviceId.Name,
                    Id = this.SerializeDeviceInfo(deviceId),
                    list = serviceResult.Result.Select(i => new { setting = i.Name, value = i.Value})
                });
            }

            return Conflict();
        }

        [HttpPost]
        [Route("myconfig")]
        public async Task<IActionResult> GetDeviceConfig([FromBody] SingleBodyParameter deviceInfo)
        {
            var deviceId = this.DeserializeDeviceInfo(deviceInfo.Body);

            var serviceResult = await this._deviceService.GetDeviceSettings(deviceId, true);

            if (serviceResult)
            {
                return new JsonResult(new
                {
                    list = serviceResult.Result.Select(i => new { setting = i.Name, value = i.Value })
                });
            }

            return Conflict();
        }

        private string SerializeDeviceInfo(DeviceIdentifier deviceId)
        {
            return this._encryptionService.Encrypt(deviceId);
        }

        private DeviceIdentifier DeserializeDeviceInfo(string deviceInfo)
        {
            return this._encryptionService.Decrypt<DeviceIdentifier>(deviceInfo);
        }

        private GroupIdentifier DeserializeGroupInfo(string groupInfo)
        {
            return this._encryptionService.Decrypt<GroupIdentifier>(groupInfo);
        }
    }
}
