using AtaRK.Mobile.Models;
using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Group;
using AtaRK.Mobile.Services.Network;
using AtaRK.Mobile.Services.Network.NetworkConnection;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;


namespace AtaRK.Mobile.ViewModels.Pages
{
    public class GroupsViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        public string ListTitleText => Localization.Groups_ListTitle;
        public string RefreshDataError => Localization.Groups_RefreshError;

        private bool pageLoaded = false;
        private bool loadingStarted = false;
        private bool listRefreshStatus = true;

        private INetworkConnectionService _networkConnectionService;
        private IAuthorizationService _authorizationService;
        private INavigationService _navigationService;
        private IGroupService _groupService;
        private IDisposable _authorizationFailedDisposable;

        private ListData<GroupInfo> lastGroupsInfo = null;

        public GroupsViewModel(
            INetworkConnectionService networkConnectionService,
            IAuthorizationService authorizationService,
            INavigationService navigationService,
            IGroupService groupService)
        {
            this._navigationService = navigationService;
            this._networkConnectionService = networkConnectionService;
            this._authorizationService = authorizationService;
            this._groupService = groupService;

            this.RefreshCommand = new Command(async () => await this.RefreshData(), () => !this.loadingStarted);

            this._authorizationFailedDisposable = this._authorizationService.AuthorizationStatusObserbavle.Where(i => !i).Subscribe(this.OnAuthorizationChanged);
        }

        public Command RefreshCommand { get; private set; }

        public bool ListRefreshedSuccessfully
        {
            get => this.listRefreshStatus;
            set
            {
                this.listRefreshStatus = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<GroupInfo> GroupInfoCollection => this.lastGroupsInfo?.Elements;

        public GroupInfo OnGroupSelected
        {
            set
            {
                if (value != null)
                {
                    _ = this.ShowGroupInfo(value);
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

            if (this.lastGroupsInfo == null)
            {
                _ = this.RefreshData();
            }
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = false;
        }

        private void OnAuthorizationChanged(bool authorizationStatus)
        {
            if (!pageLoaded || authorizationStatus)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.Login);
        }

        private async Task RefreshData()
        {
            this.loadingStarted = true;
            this.RefreshCommand.ChangeCanExecute();

            var result = await this._groupService.GetGroups();

            if (result)
            {
                this.lastGroupsInfo = result.Result;
                this.OnPropertyChanged(nameof(this.GroupInfoCollection));
            }

            this.ListRefreshedSuccessfully = result;

            this.loadingStarted = false;
            this.RefreshCommand.ChangeCanExecute();
        }

        private async Task ShowGroupInfo(GroupInfo group)
        {
            if (this.loadingStarted)
            {
                return;
            }

            this.loadingStarted = true;
            this.RefreshCommand.ChangeCanExecute();

            var serviceResult = await this._groupService.GetGroupInfo(group.GroupId);

            if (serviceResult)
            {
                this._navigationService.MoveToPage(Navigation.Pages.GroupInfo);
            }

            this.loadingStarted = false;
            this.RefreshCommand.ChangeCanExecute();
        }

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._authorizationFailedDisposable?.Dispose();
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
