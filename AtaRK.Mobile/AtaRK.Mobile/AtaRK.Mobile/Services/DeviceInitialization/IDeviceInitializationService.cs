using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DeviceInitialization
{
    public interface IDeviceInitializationService
    {
        Task<IEnumerable<DeviceInitializationData>> SearchForConnectedDevices();

        IObservable<DeviceInitializationData> DeviceConnectionOnservable { get; }
    }
}
