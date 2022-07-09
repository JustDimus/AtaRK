using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Device.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Device
{
    public class DesignTimeDeviceService : IDeviceService
    {
        private ReplaySubject<DeviceInfo> _deviceInfoSubject = new ReplaySubject<DeviceInfo>(1);

        private ReplaySubject<ChangeDeviceSettingContext> _deviceSettingSubject = new ReplaySubject<ChangeDeviceSettingContext>(1);

        private IDataManager _dataManager;

        public DesignTimeDeviceService(
            IDataManager dataManager)
        {
            this._dataManager = dataManager;
        }

        public IObservable<DeviceInfo> DeviceInfoObservable => this._deviceInfoSubject.AsObservable();

        public IObservable<ChangeDeviceSettingContext> DeviceSettingObservable => this._deviceSettingSubject.AsObservable();

        public Task<bool> AddDeviceToGroup(CreateNewDeviceContext deviceContext)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetDeviceInfo(string deviceId)
        {
            var result = await this._dataManager.GetDeviceInfo(deviceId);

            this._deviceInfoSubject.OnNext(result.Result);

            return true;
        }

        public Task<RequestContext<ListData<DeviceSetting>>> GetDeviceSettings(string deviceId)
        {
            return this._dataManager.GetDeviceSettings(deviceId);
        }

        public Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId)
        {
            return this._dataManager.GetGroupDevices(groupId);
        }

        public Task<bool> SaveDeviceSettingContext(ChangeDeviceSettingContext settingContext)
        {
            this.GetDeviceInfo(string.Empty);

            return Task.FromResult(true);
        }

        public void SetCurrentSettingChangeContext(ChangeDeviceSettingContext settingContext)
        {
            this._deviceSettingSubject.OnNext(settingContext);
        }
    }
}
