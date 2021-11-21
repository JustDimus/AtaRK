using AtaRK.Mobile.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class LoginViewModel : BaseViewModel, IPageViewModel
    {
        private INavigationService _navigationService;

        private bool isLoginProcessStarted = false;

        public LoginViewModel(
            INavigationService navigationService)
        {
            this._navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            this.LoginCommand = new Command(Login, () => !this.isLoginProcessStarted);
        }

        public string LoginButtonText => Localization.Login_Button;

        public Command LoginCommand { get; }

        public void Login()
        {
            this.isLoginProcessStarted = true;
            this.LoginCommand.ChangeCanExecute();

            this._navigationService.MoveToPage(Navigation.Pages.Registration);
        }

        public void OnPageLoaded()
        {
        }

        public void OnPageUnloaded()
        {
        }
    }
}
