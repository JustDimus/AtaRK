using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Credentials;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Device;
using AtaRK.Mobile.Services.DeviceInitialization;
using AtaRK.Mobile.Services.Group;
using AtaRK.Mobile.Services.Group.UserRole;
using AtaRK.Mobile.Services.Implementations;
using AtaRK.Mobile.Services.Network;
using AtaRK.Mobile.Services.Network.NetworkConnection;
using AtaRK.Mobile.Services.Network.Service;
using AtaRK.Mobile.Services.Serializer;
using AtaRK.Mobile.ViewModels.Pages;
using AtaRK.Mobile.Views.Pages;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AtaRK.Mobile.ViewModels
{
    public class ApplicationLocator : IDisposable
    {
        private Task pageViewModelsTask;

        private TinyIoCContainer container = new TinyIoCContainer();

        public ApplicationLocator()
        {
            container.Register<INavigationService, NavigationService>().AsSingleton();
            container.Register<MainViewModel>().AsSingleton();
            container.Register<IntroViewModel>().AsSingleton();
            container.Register<LoginViewModel>().AsSingleton();
            container.Register<GroupsViewModel>().AsSingleton();
            container.Register<GroupInfoViewModel>().AsSingleton();
            container.Register<DeviceInfoViewModel>().AsSingleton();
            container.Register<RegistrationViewModel>().AsSingleton();
            container.Register<ChangeSettingViewModel>().AsSingleton();
            container.Register<DeviceInitializationViewModel>().AsSingleton();
            container.Register<IApplicationProperties, ApplicationProperties>().AsSingleton();
            container.Register<INetworkService, NetworkService>().AsSingleton();
            container.Register<NetworkSettings>().AsSingleton().AsSingleton();
            container.Register<ISerializer, Serializer>().AsSingleton();
            container.Register<IUserRoleManager, UserRoleManager>();
            container.Register<IDeviceInitializationService, DeviceInitializationService>();
#if STUB
            container.Register<ICredentialsManager, DesignTimeCredentialsManager>().AsSingleton();
            container.Register<IAuthorizationService, DesignTimeAuthorizationService>().AsSingleton();
            container.Register<INetworkConnectionService, DesignTimeNetworkConnectionService>().AsSingleton();
            container.Register<IDataManager, DesignTimeDataManager>().AsSingleton();
            container.Register<IGroupService, DesignTimeGroupService>().AsSingleton();
            container.Register<IDeviceService, DesignTimeDeviceService>().AsSingleton();
#elif DEV
            container.Register<ICredentialsManager, CredentialsManager>();
            container.Register<IAuthorizationService, AuthorizationService>().AsSingleton();
            container.Register<INetworkConnectionService, NetworkConnectionService>().AsSingleton();
            container.Register<IDataManager, NetworkDataManager>().AsSingleton();
            container.Register<IGroupService, DesignTimeGroupService>().AsSingleton();
            container.Register<IDeviceService, DeviceService>().AsSingleton();
#endif

            this.MainPageViewModel = container.Resolve<MainViewModel>();
            this.IntroPageViewModel = container.Resolve<IntroViewModel>();

            this.pageViewModelsTask = Task.Run(() =>
            {
                this.LoginPageViewModel = container.Resolve<LoginViewModel>();
                this.RegistrationPageViewModel = container.Resolve<RegistrationViewModel>();
                this.GroupsPageViewModel = container.Resolve<GroupsViewModel>();
                this.GroupInfoPageViewModel = container.Resolve<GroupInfoViewModel>();
                this.DeviceInfoPageViewModel = container.Resolve<DeviceInfoViewModel>();
                this.ChangeSettingPageViewModel = container.Resolve<ChangeSettingViewModel>();
                this.DeviceInitializationPageViewModel = container.Resolve<DeviceInitializationViewModel>();
                this.IntroPageViewModel.AllPagesViewModelsLoaded = true;
            });
        }

        public MainViewModel MainPageViewModel { get; private set; }

        public LoginViewModel LoginPageViewModel { get; private set; }

        public IntroViewModel IntroPageViewModel { get; private set; }

        public RegistrationViewModel RegistrationPageViewModel { get; private set; }

        public GroupsViewModel GroupsPageViewModel { get; private set; }

        public GroupInfoViewModel GroupInfoPageViewModel { get; private set; }

        public DeviceInfoViewModel DeviceInfoPageViewModel { get; private set; }

        public ChangeSettingViewModel ChangeSettingPageViewModel { get; private set; }

        public DeviceInitializationViewModel DeviceInitializationPageViewModel { get; private set; }

        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                this.pageViewModelsTask.Dispose();
                this.container?.Dispose();

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
