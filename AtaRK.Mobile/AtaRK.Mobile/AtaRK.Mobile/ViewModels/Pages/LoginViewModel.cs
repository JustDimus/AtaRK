using AtaRK.Mobile.Navigation;
using AtaRK.Mobile.Services.Authorization;
using AtaRK.Mobile.Services.Credentials;
using AtaRK.Mobile.Services.Network;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Localization = AtaRK.Mobile.Resources.Texts.ApplicationLocalization;

namespace AtaRK.Mobile.ViewModels.Pages
{
    public class LoginViewModel : BaseViewModel, IPageViewModel, IDisposable
    {
        private INavigationService _navigationService;
        private INetworkConnectionService _networkConnection;
        private ICredentialsManager _credentialsManager;
        private IAuthorizationService _authorizationService;

        private bool isLoginProcessStarted = false;
        private bool lastAuthorizationStatus = false;
        private bool loginError = false;
        private bool pageLoaded = false;

        private IDisposable networkStateChangeDisposable;
        private IDisposable authorizationStatusDisposable;

        public LoginViewModel(
            INavigationService navigationService,
            INetworkConnectionService networkConnectionService,
            ICredentialsManager credentialsManager,
            IAuthorizationService authorizationService)
        {
            this._navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this._networkConnection = networkConnectionService ?? throw new ArgumentNullException(nameof(networkConnectionService));
            this._credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            this.LoginCommand = new Command(async () => await this.Login(), () => this.CanLogin);

            this.networkStateChangeDisposable = this._networkConnection.Subscribe(this.OnNetworkConnectionChanged);
            this.authorizationStatusDisposable = this._authorizationService.AuthorizationStatusObserbavle.Where(i => i).Subscribe(this.OnAuthorizationStatusChanged);
        }

        public string LoginButtonText => Localization.Login_Button;
        public string EmailLabelText => Localization.Login_Email;
        public string PasswordLabelText => Localization.Login_Password;

        private string emailField = string.Empty;
        private string passwordField = string.Empty;

        public string EmailField
        {
            get => this.emailField;
            set
            {
                this.emailField = value;
                this.OnPropertyChanged();
                this.LoginCommand.ChangeCanExecute();
            }
        }

        public string PasswordField
        {
            get => this.passwordField;
            set
            {
                this.passwordField = value;
                this.OnPropertyChanged();
                this.LoginCommand.ChangeCanExecute();
            }
        }

        public Command LoginCommand { get; }

        public string ErrorText { get; private set; }

        public bool NetworkConnectionStatus => this._networkConnection.NetworkStatus;

        public bool ShowError => !this.NetworkConnectionStatus || loginError;

        private bool CredentialsEntered => !string.IsNullOrWhiteSpace(this.EmailField) && !string.IsNullOrWhiteSpace(this.PasswordField);

        private bool CanLogin => !this.isLoginProcessStarted && this.NetworkConnectionStatus && CredentialsEntered;

        private void OnNetworkConnectionChanged(bool networkStatus)
        {
            if (!networkStatus)
            {
                this.ErrorText = Localization.NetworkConnection_Error;
                this.OnPropertyChanged(nameof(this.ErrorText));
            }

            this.OnPropertyChanged(nameof(this.ShowError));
            this.LoginCommand.ChangeCanExecute();
        }

        public void OnAuthorizationStatusChanged(bool authorizationStatus)
        {
            this.lastAuthorizationStatus = authorizationStatus;

            if (!this.pageLoaded || !authorizationStatus)
            {
                return;
            }

            this._navigationService.MoveToPage(Navigation.Pages.Groups);
        }

        public async Task Login()
        {
            var loginData = new LoginData()
            {
                Email = this.emailField,
                Password = this.passwordField
            };

            this.isLoginProcessStarted = true;
            this.LoginCommand.ChangeCanExecute();

            var result = await this._authorizationService.LoginAsync(loginData);

            if (!result)
            {
                this.ErrorText = Localization.Login_Error;
                this.loginError = true;
                this.OnPropertyChanged(nameof(this.ShowError));
            }

            this.isLoginProcessStarted = false;
            this.LoginCommand.ChangeCanExecute();
        }

        public void OnPageLoaded()
        {
            if (this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = true;

            this.OnAuthorizationStatusChanged(this.lastAuthorizationStatus);
        }

        public void OnPageUnloaded()
        {
            if (!this.pageLoaded)
            {
                return;
            }

            this.pageLoaded = false;
        }

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.networkStateChangeDisposable?.Dispose();
                    this.authorizationStatusDisposable?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LoginViewModel()
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
