using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Views;
using AtaRK.Mobile.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using TextLocalization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels
{
    public class MainViewModel : IDisposable
    {
        private INavigationService _navigationService;

        private List<Route> routes = new List<Route>();

        private IDisposable changePageDisposable;

        public MainViewModel(
            INavigationService navigationService)
        {
            this._navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void Start()
        {
            this.changePageDisposable = this._navigationService.CurrentPageKey
                .Subscribe((page) => Device.BeginInvokeOnMainThread(async () =>
            {
                var nextPage = this.CreatePage(page);

                if (nextPage != null)
                {
                    await Navigation.PushAsync(nextPage).ConfigureAwait(true);

                    if (this.Navigation.NavigationStack.Count > 0)
                    {
                        foreach (var i in Navigation.NavigationStack.Where(i => i != nextPage).ToList())
                        {
                            Navigation.RemovePage(i);
                        }
                    }
                }
            }));
        }

        public INavigation Navigation { get; set; }

        private BasePage CreatePage(Navigation.Pages page)
        {
            BasePage resultPage = null;

            switch (page)
            {
                case Mobile.Navigation.Pages.Intro:
                    resultPage = new IntroPage();
                    break;
                case Mobile.Navigation.Pages.Login:
                    resultPage = new LoginPage();
                    break;
                case Mobile.Navigation.Pages.Registration:
                    resultPage = new RegisterPage();
                    break;
                case Mobile.Navigation.Pages.Groups:
                    resultPage = new GroupsPage();
                    break;
                case Mobile.Navigation.Pages.GroupInfo:
                    resultPage = new GroupInfoPage();
                    break;
                case Mobile.Navigation.Pages.DeviceInfo:
                    resultPage = new DeviceInfoPage();
                    break;
                case Mobile.Navigation.Pages.ChangeDeviceSetting:
                    resultPage = new ChangeSettingPage();
                    break;
                case Mobile.Navigation.Pages.DeviceInitialization:
                    resultPage = new DeviceInitializationPage();
                    break;
                default:
                    break;
            }

            return resultPage;
        }

        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MainViewModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}