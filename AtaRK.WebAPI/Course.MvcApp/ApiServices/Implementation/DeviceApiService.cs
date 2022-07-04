using Course.MvcApp.Models.MvcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices.Implementation
{
    public class DeviceApiService : BaseApiService, IDeviceApiService
    {
        public DeviceApiService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public async Task<DeviceShowModel> GetDeviceInformation(string deviceId)
        {
            var requestResult = await this.SendRequestAsync<DeviceShowModel>(
                "device/info",
                RequestMethod.POST,
                new { body = deviceId });

            return requestResult;
        }

        public async Task<ListModel<DeviceModel>> GetDevicesInOrganization(string organizationId)
        {
            var requestResult = await this.SendRequestAsync<ListModel<DeviceModel>>(
                "device/list",
                RequestMethod.POST,
                new { body = organizationId });

            return requestResult;
        }

        public async Task<bool> UpdateDeviceSettings(DeviceUpdateSettingModel updateSettingModel)
        {
            var requestResult = await this.SendRequestWithoutResponseAsync(
                "device/update",
                RequestMethod.POST,
                new
                {
                    device_id = updateSettingModel.DeviceId,
                    settings = new object[]
                    {
                        new
                        {
                            setting = updateSettingModel.Setting,
                            value = updateSettingModel.Value
                        }
                    },
                    is_admin_only = updateSettingModel.IsReadonly
                });

            return requestResult;
        }
    }
}
