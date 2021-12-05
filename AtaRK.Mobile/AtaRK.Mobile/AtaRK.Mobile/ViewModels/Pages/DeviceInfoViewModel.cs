using AtaRK.Mobile.Models;
using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Device;
using AtaRK.Mobile.Services.Device.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class DeviceInfoViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        public string DeviceSettingsListTitle => Localization.DeviceInfo_Settings;
        public string SettingsListRefreshError => Localization.DeviceInfo_RefreshError;
        public string SettingTitleText => Localization.DeviceInfo_SettingTitle;
        public string SettingValueText => Localization.DeviceInfo_ValueTitle;

        private IDeviceService _deviceService;
        private INavigationService _navigationService;
        private IAuthorizationService _authorizationService;

        private bool pageLoaded = false;
        private bool settingsRefreshStatus = true;
        private DeviceInfo lastDeviceInfo = null;
        private ListData<DeviceSetting> lastDeviceSettingList = null;

        private IDisposable deviceChangedDisposable;
        private IDisposable authorizationChangedDisposable;

        public DeviceInfoViewModel(
            INavigationService navigationService,
            IDeviceService deviceService,
            IAuthorizationService authorizationService)
        {
            this._navigationService = navigationService;
            this._deviceService = deviceService;
            this._authorizationService = authorizationService;

            this.GoBackToGroupCommand = new Command(this.ReturnToGroupInfoPage);

            this.deviceChangedDisposable = this._deviceService.DeviceInfoObservable.Subscribe(async (i) => await this.OnCurrentDeviceChanged(i));
            this.authorizationChangedDisposable = this._authorizationService.AuthorizationStatusObserbavle.Subscribe(this.OnAuthorizationChanged);
        }

        public Command GoBackToGroupCommand { get; private set; }

        private string deviceTitle;
        public string DeviceTitle
        {
            get => this.deviceTitle;
            set
            {
                this.deviceTitle = value;
                OnPropertyChanged();
            }
        }

        public bool ListRefreshedSuccessfully
        {
            get => this.settingsRefreshStatus;
            set
            {
                this.settingsRefreshStatus = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<DeviceSetting> DeviceSettingsCollection => this.lastDeviceSettingList?.Elements;

        public DeviceSetting OnSettingSelected
        {
            set
            {
                if (value != null)
                {
                    this.ChangeDeviceSetting(value);
                }
            }
        }

        public void OnPageLoaded()
        {
            if (this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = true;
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = false;
        }

        private async Task OnCurrentDeviceChanged(DeviceInfo device)
        {
            if (device == null)
            {
                return;
            }

            this.lastDeviceInfo = device;

            this.DeviceTitle = string.Concat(Localization.DeviceInfo_Title, " ", device.DeviceType);

            await this.RefreshDeviceSettings();
        }

        private void OnAuthorizationChanged(bool authorizationStatus)
        {
            if (!this.pageLoaded || authorizationStatus)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.Login);
        }

        private async Task RefreshDeviceSettings()
        {
            if (this.lastDeviceInfo == null)
            {
                return;
            }

            var deviceSettings = await this._deviceService.GetDeviceSettings(this.lastDeviceInfo.Id);

            if (deviceSettings)
            {
                this.lastDeviceSettingList = deviceSettings.Result;
                OnPropertyChanged(nameof(this.DeviceSettingsCollection));
            }

            this.ListRefreshedSuccessfully = deviceSettings;
        }

        private void ChangeDeviceSetting(DeviceSetting setting)
        {
            var settingContext = new ChangeDeviceSettingContext()
            {
                DeviceId = this.lastDeviceInfo.Id,
                Setting = setting.Setting,
                Value = setting.Value
            };

            this._deviceService.SetCurrentSettingChangeContext(settingContext);

            this._navigationService.MoveToPage(Navigation.Pages.ChangeDeviceSetting);
        }

        private void ReturnToGroupInfoPage()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.GroupInfo);
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.deviceChangedDisposable?.Dispose();
                    this.authorizationChangedDisposable?.Dispose();
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
