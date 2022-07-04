using Course.MvcApp.ApiServices;
using Course.MvcApp.Models.MvcModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.MvcControllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceApiService _deviceApiService;

        public DeviceController(IDeviceApiService deviceApiService)
        {
            _deviceApiService = deviceApiService ?? throw new ArgumentNullException(nameof(deviceApiService));
        }

        [HttpGet]
        [Route("Show")]
        public async Task<IActionResult> Show([FromQuery] string deviceId)
        {
            var requestResult = await this._deviceApiService.GetDeviceInformation(deviceId);

            return View(requestResult);
        }

        [HttpGet]
        [Route("Update")]
        public async Task<IActionResult> Update(
            [FromQuery] string deviceId,
            [FromQuery] string setting = null,
            [FromQuery] string value = null)
        {
            var deviceUpdateModel = new DeviceUpdateSettingModel()
            {
                DeviceId = deviceId,
                Setting = setting,
                Value = value,
                IsReadonly = !string.IsNullOrEmpty(setting)
            };

            return View(deviceUpdateModel);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] DeviceUpdateSettingModel deviceUpdateModel)
        {
            var result = await this._deviceApiService.UpdateDeviceSettings(deviceUpdateModel);

            if (!result)
            {
                return View(deviceUpdateModel);
            }

            return RedirectToAction("Show", new { deviceId = deviceUpdateModel.DeviceId });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
