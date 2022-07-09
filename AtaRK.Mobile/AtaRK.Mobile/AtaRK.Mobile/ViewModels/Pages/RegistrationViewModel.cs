using AtaRK.Mobile.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class RegistrationViewModel : BaseViewModel, IPageViewModel
    {
        private readonly INavigationService _navigationService;

        public RegistrationViewModel(
            INavigationService navigationService)
        {
            this._navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void OnPageLoaded()
        {

        }

        public void OnPageUnloaded()
        {

        }
    }
}
