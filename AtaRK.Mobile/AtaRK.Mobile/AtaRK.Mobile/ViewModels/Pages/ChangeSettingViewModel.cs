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
    public class ChangeSettingViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        public string SaveSettingErrorText => Localization.ChangeDeviceSetting_SaveError;
        public string SettingTitleText => Localization.DeviceInfo_SettingTitle;
        public string ValueTitleText => Localization.DeviceInfo_ValueTitle;
        public string ChangeSettingText => Localization.ChangeDeviceSetting_Title;
        public string SaveButtonText => Localization.ChangeDeviceSetting_SaveButton;

        private INavigationService _navigationService;
        private IDeviceService _deviceService;
        private IAuthorizationService _authorizationService;

        private bool pageLoaded = false;
        private bool saveProcessStarted = false;
        private bool saveSettingsError = false;
        private bool isSettingReadonly = false;

        private ChangeDeviceSettingContext lastDeviceSettingContext = null;

        private IDisposable settingContextChangedDisposable = null;
        private IDisposable authorizationChangedDisposable = null;

        public ChangeSettingViewModel(
            INavigationService navigationService,
            IDeviceService deviceService,
            IAuthorizationService authorizationService)
        {
            this._navigationService = navigationService;
            this._deviceService = deviceService;
            this._authorizationService = authorizationService;

            this.GoBackToDeviceInfoCommand = new Command(this.ReturnToDeviceInfoPage);
            this.ApplySettingCommand = new Command(async () => await this.ApplySetting(), () => !this.saveProcessStarted);

            this.settingContextChangedDisposable = this._deviceService.DeviceSettingObservable.Subscribe(this.OnDeviceSettingContextChanged);
            this.authorizationChangedDisposable = this._authorizationService.AuthorizationStatusObserbavle.Subscribe(this.OnAuthorizationChanged);
        }

        private string currentSetting;
        public string CurrentSetting
        {
            get => this.currentSetting;
            set
            {
                this.currentSetting = value;
                this.OnPropertyChanged();
            }
        }
        
        private string currentValue;
        public string CurrentValue
        {
            get => this.currentValue;
            set
            {
                this.currentValue = value;
                this.OnPropertyChanged();
            }
        }

        public bool SaveSettingError
        {
            get => this.saveSettingsError;
            set
            {
                this.saveSettingsError = value;
                OnPropertyChanged();
            }
        }

        public bool IsSettingReadonly
        {
            get => this.isSettingReadonly;
            set
            {
                this.isSettingReadonly = value;
                OnPropertyChanged();
            }
        }

        public Command GoBackToDeviceInfoCommand { get; private set; }

        public Command ApplySettingCommand { get; private set; }

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

        private void ReturnToDeviceInfoPage()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.DeviceInfo);
        }

        private void OnDeviceSettingContextChanged(ChangeDeviceSettingContext settingContext)
        {
            this.lastDeviceSettingContext = settingContext;

            this.CurrentSetting = settingContext.Setting;
            this.CurrentValue = settingContext.Value;
            

            this.isSettingReadonly = !string.IsNullOrEmpty(settingContext.Setting);
        }

        private async Task ApplySetting()
        {
            if (this.saveProcessStarted)
            {
                return;
            }

            this.saveProcessStarted = true;
            this.ApplySettingCommand.ChangeCanExecute();

            var settingContext = new ChangeDeviceSettingContext()
            {
                DeviceId = this.lastDeviceSettingContext.DeviceId,
                Setting = this.CurrentSetting,
                Value = this.CurrentValue
            };

            var serviceResult = await this._deviceService.SaveDeviceSettingContext(settingContext);

            if (serviceResult)
            {
                this._navigationService.MoveToPage(Navigation.Pages.DeviceInfo);
            }

            this.SaveSettingError = !serviceResult;

            this.saveProcessStarted = false;
            this.ApplySettingCommand.ChangeCanExecute();
        }

        private void OnAuthorizationChanged(bool authorizationStatus)
        {
            if (!this.pageLoaded || authorizationStatus)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.Login);
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.settingContextChangedDisposable?.Dispose();
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
