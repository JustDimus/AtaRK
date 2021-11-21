using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services;
using AtaRK.Mobile.Services.Implementations;
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
            container.Register<RegistrationViewModel>().AsSingleton();
            container.Register<IApplicationProperties, ApplicationProperties>();

            this.MainPageViewModel = container.Resolve<MainViewModel>();
            this.IntroPageViewModel = container.Resolve<IntroViewModel>();

            this.pageViewModelsTask = Task.Run(() =>
            {
                this.LoginPageViewModel = container.Resolve<LoginViewModel>();
                this.RegistrationPageViewModel = container.Resolve<RegistrationViewModel>();
                this.IntroPageViewModel.AllPagesViewModelsLoaded = true;
            });
        }

        public MainViewModel MainPageViewModel { get; private set; }

        public LoginViewModel LoginPageViewModel { get; private set; }

        public IntroViewModel IntroPageViewModel { get; private set; }

        public RegistrationViewModel RegistrationPageViewModel { get; private set; }

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

                disposedValue = true;
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
