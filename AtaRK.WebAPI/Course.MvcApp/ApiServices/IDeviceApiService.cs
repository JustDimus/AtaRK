using Course.MvcApp.Models.MvcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices
{
    public interface IDeviceApiService
    {
        Task<ListModel<DeviceModel>> GetDevicesInOrganization(string organizationId);

        Task<DeviceShowModel> GetDeviceInformation(string deviceId);

        Task<bool> UpdateDeviceSettings(DeviceUpdateSettingModel updateSettingModel);
    }
}
