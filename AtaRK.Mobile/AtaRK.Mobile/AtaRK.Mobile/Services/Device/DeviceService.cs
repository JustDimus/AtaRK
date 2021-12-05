using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Device.Models;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Device
{
    public class DeviceService : IDeviceService, IDisposable
    {
        private IDataManager _dataManager;

        private ReplaySubject<DeviceInfo> deviceInfoSubject = new ReplaySubject<DeviceInfo>(1);
        private ReplaySubject<ChangeDeviceSettingContext> deviceSettingSubject = new ReplaySubject<ChangeDeviceSettingContext>(1);

        public DeviceService(
            IDataManager dataManager)
        {
            this._dataManager = dataManager;
        }

        public IObservable<DeviceInfo> DeviceInfoObservable => this.deviceInfoSubject.AsObservable();

        public IObservable<ChangeDeviceSettingContext> DeviceSettingObservable => this.deviceSettingSubject.AsObservable();

        public async Task<bool> GetDeviceInfo(string deviceId)
        {
            var result = await this._dataManager.GetDeviceInfo(deviceId);

            if (result)
            {
                this.deviceInfoSubject.OnNext(result.Result);
            }

            return result;
        }

        public Task<RequestContext<ListData<DeviceSetting>>> GetDeviceSettings(string deviceId)
        {
            return this._dataManager.GetDeviceSettings(deviceId);
        }

        public Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId)
        {
            return this._dataManager.GetGroupDevices(groupId);
        }

        public async Task<bool> SaveDeviceSettingContext(ChangeDeviceSettingContext settingContext)
        {
            var saveResult = await this._dataManager.SaveDeviceSetting(settingContext);

            if (saveResult)
            {
                return await this.GetDeviceInfo(settingContext.DeviceId);
            }

            return saveResult;
        }

        public void SetCurrentSettingChangeContext(ChangeDeviceSettingContext settingContext)
        {
            this.deviceSettingSubject.OnNext(settingContext);
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.deviceInfoSubject?.Dispose();
                    this.deviceSettingSubject?.Dispose();
                }

                //Unmanaged resources

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
