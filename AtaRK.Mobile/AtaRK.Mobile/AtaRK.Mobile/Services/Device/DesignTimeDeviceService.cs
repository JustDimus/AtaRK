using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Device
{
    public class DesignTimeDeviceService : IDeviceService
    {
        private ReplaySubject<DeviceInfo> _deviceInfoSubject = new ReplaySubject<DeviceInfo>(1);

        private IDataManager _dataManager;

        public DesignTimeDeviceService(
            IDataManager dataManager)
        {
            this._dataManager = dataManager;
        }

        public IObservable<DeviceInfo> DeviceInfoObservable => this._deviceInfoSubject.AsObservable();

        public Task<bool> GetDeviceInfo(string deviceId)
        {
            var deviceInfo = new DeviceInfo()
            {
                Id = deviceId,
                DeviceCode = "Device code here",
                DeviceType = "Device type here"
            };

            this._deviceInfoSubject.OnNext(deviceInfo);

            return Task.FromResult(true);
        }

        public Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId)
        {
            return this._dataManager.GetGroupDevices(groupId);
        }
    }
}
