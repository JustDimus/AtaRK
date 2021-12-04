using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services;
using AtaRK.Mobile.Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class IntroViewModel : IPageViewModel, IDisposable
    {
        private const int MAX_WAIT_TIME = 150;

        private bool allViewModelsLoaded = false;
        
        private IBackgroundWorkerWrapper _backgroundWorker;
        private readonly INavigationService _navigationService;
        private readonly IApplicationProperties _applicationProperties;

        public IntroViewModel(
            INavigationService navigationService,
            IApplicationProperties applicationProperties)
        {
            this._navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this._applicationProperties = applicationProperties ?? throw new ArgumentNullException(nameof(applicationProperties));
        }

        public string WelcomeText => Localization.Intro_Welcome;
        public string DescriptionText => Localization.Intro_Description;
        public string LoadingText => Localization.Loading;

        public bool AllPagesViewModelsLoaded
        {
            get => this.allViewModelsLoaded;
            set
            {
                this.allViewModelsLoaded = value;
            }
        }        

        public void OnPageLoaded()
        {
            this._backgroundWorker = new BackgroundWorkerWrapper(this.WaitInitializingViewModels, (sender, e) => this.MoveToNextPage());
            this._backgroundWorker.RunWorkerAsync();
        }

        public void OnPageUnloaded()
        {
        }

        private void WaitInitializingViewModels(object sender, DoWorkEventArgs e)
        {
            int counter = 0;
            do
            {
                Task.Delay(1000).Wait();
            } while (!this.allViewModelsLoaded && counter++ < MAX_WAIT_TIME);
        }

        private void MoveToNextPage()
        {
            if (!this.allViewModelsLoaded)
            {
                this._applicationProperties.AddOrUpdate(ApplicationPhase.Loading, ApplicationStatus.FAILED);
                this._navigationService.MoveToPage(Navigation.Pages.Error);
            }

            this._navigationService.MoveToPage(Navigation.Pages.Login);
        }


        #region IDisposable
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._backgroundWorker?.Dispose();
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~IntroViewModel()
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
