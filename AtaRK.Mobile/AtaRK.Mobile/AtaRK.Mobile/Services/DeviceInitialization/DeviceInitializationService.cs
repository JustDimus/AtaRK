using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DeviceInitialization
{
    public class DeviceInitializationService : IDeviceInitializationService
    {
        public IObservable<DeviceInitializationData> DeviceConnectionOnservable => Observable.Empty<DeviceInitializationData>();

        public async Task<IEnumerable<DeviceInitializationData>> SearchForConnectedDevices()
        {
            return new List<DeviceInitializationData>()
            {
                new DeviceInitializationData()
                {
                    DeviceCode = Guid.NewGuid().ToString(),
                    DeviceType = "SARA-3"
                }
            };
        }
    }
}
