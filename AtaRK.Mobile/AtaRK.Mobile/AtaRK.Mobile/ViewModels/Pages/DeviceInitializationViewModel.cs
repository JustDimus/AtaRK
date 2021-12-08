using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Device;
using AtaRK.Mobile.Services.Device.Models;
using AtaRK.Mobile.Services.DeviceInitialization;
using AtaRK.Mobile.Services.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class DeviceInitializationViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        public string DeviceCreationErrorText => Localization.DeviceInitialization_CannotAdd;
        public string DeviceCodeText => Localization.DeviceInitialization_DeviceCode;
        public string DeviceTypeText => Localization.DeviceInitialization_DeviceType;
        public string DeviceTitleText => Localization.DeviceInitialization_Title;
        public string AddDeviceButtonText => Localization.DeviceInitialization_AddButton;

        private bool pageLoaded = false;
        private bool searchStarted = false;
        private bool deviceCreatedStatus = true;

        private GroupInformation lastGroupInformation = null;

        private IAuthorizationService _authorizationService;
        private IDeviceService _deviceService;
        private IGroupService _groupService;
        private IDeviceInitializationService _initializationService;
        private INavigationService _navigationService;

        private IDisposable groupInfoDisposable;

        public DeviceInitializationViewModel(
            IAuthorizationService authorizationService,
            IDeviceService deviceService,
            IGroupService groupService,
            IDeviceInitializationService initializationService,
            INavigationService navigationService)
        {
            this._authorizationService = authorizationService;
            this._deviceService = deviceService;
            this._groupService = groupService;
            this._initializationService = initializationService;
            this._navigationService = navigationService;

            this.SearchCommand = new Command(async () => await this.SearchForConnectedDevices(), () => !this.searchStarted);
            this.AddDeviceToGroupCommand = new Command(async () => await this.AddDeviceToGroup(), () => this.AnyDeviceConnected && lastGroupInformation != null);
            this.GoBackToGroupInfoCommand = new Command(this.ReturnToGroupInfoPage);

            this.groupInfoDisposable = this._groupService.GroupInfoObservable.Subscribe(this.OnGroupInfoChanged);
        }

        public Command AddDeviceToGroupCommand { get; private set; }
        public Command SearchCommand { get; private set; }
        public Command GoBackToGroupInfoCommand { get; private set; }

        public bool DeviceCreatedStatus
        {
            get => this.deviceCreatedStatus;
            set
            {
                this.deviceCreatedStatus = value;
                OnPropertyChanged();
            }
        }

        private bool deviceConnected;
        public bool AnyDeviceConnected
        {
            get => this.deviceConnected;
            set
            {
                this.deviceConnected = value;
                OnPropertyChanged();
            }
        }

        private string deviceCode;
        public string DeviceCode
        {
            get => this.deviceCode;
            set
            {
                this.deviceCode = value;
                OnPropertyChanged();
            }
        }

        private string deviceType;
        public string DeviceType
        {
            get => this.deviceType;
            set
            {
                this.deviceType = value;
                OnPropertyChanged();
            }
        }

        public void OnPageLoaded()
        {
            if (this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = true;

            this.ClearDevicePage();

            _ = this.SearchForConnectedDevices();
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = false;
        }

        private void OnGroupInfoChanged(GroupInformation groupInfo)
        {
            this.lastGroupInformation = groupInfo;
            this.AddDeviceToGroupCommand.ChangeCanExecute();
        }

        private async Task SearchForConnectedDevices()
        {
            this.searchStarted = true;
            this.SearchCommand.ChangeCanExecute();

            this.ClearDevicePage();

            var connectedDevice = (await this._initializationService.SearchForConnectedDevices()).FirstOrDefault();

            if (connectedDevice != null)
            {
                this.DeviceCode = connectedDevice.DeviceCode;
                this.DeviceType = connectedDevice.DeviceType;
            }

            this.AnyDeviceConnected = connectedDevice != null;
            this.AddDeviceToGroupCommand.ChangeCanExecute();

            this.searchStarted = false;
            this.SearchCommand.ChangeCanExecute();
        }

        private void ClearDevicePage()
        {
            this.DeviceCode = string.Empty;
            this.DeviceType = string.Empty;
            this.AnyDeviceConnected = false;
            this.AddDeviceToGroupCommand.ChangeCanExecute();
        }

        private async Task AddDeviceToGroup()
        {
            if (!this.AnyDeviceConnected || this.lastGroupInformation == null)
            {
                return;
            }

            var deviceContext = new CreateNewDeviceContext()
            {
                GroupId = this.lastGroupInformation.GroupId,
                DeviceCode = this.DeviceCode,
                DeviceType = this.DeviceType
            };

            var result = await this._deviceService.AddDeviceToGroup(deviceContext);

            if (result)
            {
                this.ReturnToGroupInfoPage();
                this.deviceCreatedStatus = false;
                this.DeviceCreatedStatus = true;
            }
            else
            {
                this.deviceCreatedStatus = true;
                this.DeviceCreatedStatus = false;
            }
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
                    //Managed resources
                    this.groupInfoDisposable?.Dispose();
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
