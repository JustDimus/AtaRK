using AtaRK.Mobile.Models;
using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services.Device;
using AtaRK.Mobile.Services.Group;
using AtaRK.Mobile.Services.Group.UserRole;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class GroupInfoViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        public string DeviceListTitleText => Localization.GroupInfo_ListTitle;
        public string DeviceListRefreshError => Localization.GroupInfo_RefreshError;

        private INavigationService _navigationService;
        private IGroupService _groupService;
        private IDeviceService _deviceService;

        private bool pageLoaded = false;
        private bool refreshProcessStarted = false;
        private bool listRefreshStatus = true;

        private GroupInformation lastGroupInformation = null;

        private ListData<DeviceInfo> lastDeviceList = null;

        private IDisposable groupInfoDisposable;

        public GroupInfoViewModel(
            INavigationService navigationService,
            IGroupService groupService,
            IDeviceService deviceService)
        {
            this._groupService = groupService;
            this._navigationService = navigationService;
            this._deviceService = deviceService;

            this.GoBackToGroupsCommand = new Command(this.ReturnToGroupsPage);
            this.RefreshDeviceListCommand = new Command(async () => await this.RefreshDevicesList(), () => !this.refreshProcessStarted);

            this.groupInfoDisposable = this._groupService.GroupInfoObservable.Subscribe(async (i) => await this.OnGroupInfoChanged(i));
        }

        public IEnumerable<DeviceInfo> DeviceList => this.lastDeviceList?.Elements;

        public Command GoBackToGroupsCommand { get; private set; }

        public Command RefreshDeviceListCommand { get; private set; }

        public DeviceInfo OnDeviceSelected
        {
            set
            {
                if (value != null)
                {
                    _ = this.ShowDeviceInfo(value);
                }
            }
        }

        private string groupName;
        public string GroupName
        {
            get => this.groupName;
            set
            {
                this.groupName = value;
                this.OnPropertyChanged();
            }
        }

        private string userRole;
        public string UserRole
        {
            get => this.userRole;
            set
            {
                this.userRole = value;
                this.OnPropertyChanged();
            }
        }

        public bool ListRefreshedSuccessfully
        {
            get => this.listRefreshStatus;
            set
            {
                this.listRefreshStatus = value;
                OnPropertyChanged();
            }
        }

        public bool RefreshProcessStatus
        {
            get => this.listRefreshStatus;
            set
            {
                this.refreshProcessStarted = value;
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
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = true;
        }

        private async Task OnGroupInfoChanged(GroupInformation groupInfo)
        {
            this.lastGroupInformation = groupInfo;

            this.GroupName = groupInfo.GroupName.Length <= 20 ? groupInfo.GroupName : string.Concat(groupInfo.GroupName.Substring(0, 20), "...");

            var userRoleString = $"UserRole_{Enum.GetName(typeof(UserRole), groupInfo.UserRole)}";

            this.UserRole = Localization.ResourceManager.GetString(userRoleString, Localization.Culture);

            await this.RefreshDevicesList();
        }

        private void ReturnToGroupsPage()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.Groups);
        }

        private async Task RefreshDevicesList()
        {
            this.RefreshProcessStatus = true;
            this.RefreshDeviceListCommand.ChangeCanExecute();

            var serviceResult = await this._deviceService.GetGroupDevices(this.lastGroupInformation.GroupId);

            if (serviceResult)
            {
                this.lastDeviceList = serviceResult.Result;
                this.OnPropertyChanged(nameof(this.DeviceList));
            }

            this.ListRefreshedSuccessfully = serviceResult;

            this.RefreshProcessStatus = false;
            this.RefreshDeviceListCommand.ChangeCanExecute();
        }

        private async Task ShowDeviceInfo(DeviceInfo device)
        {
            if (this.refreshProcessStarted)
            {
                return;
            }

            var serviceResult = await this._deviceService.GetDeviceInfo(device.Id);

            if (serviceResult)
            {
                this._navigationService.MoveToPage(Navigation.Pages.DeviceInfo);
            }

            this.refreshProcessStarted = false;
            this.RefreshDeviceListCommand.ChangeCanExecute();
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
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
