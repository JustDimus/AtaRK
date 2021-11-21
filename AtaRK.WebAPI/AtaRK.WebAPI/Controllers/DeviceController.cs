using AtaRK.BLL.Interfaces;
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

        public DeviceController(
            IDeviceService deviceService)
        {
            this._deviceService = deviceService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddDevice([FromBody] AddDeviceVM deviceInfo)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> RemoveDevice([FromBody] SingleBodyParameter deviceInfo)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateDeviceSettingsVM updateInfo)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> GetDeviceSettings([FromBody] SingleBodyParameter deviceInfo)
        {
            throw new NotImplementedException();
        }
    }
}
