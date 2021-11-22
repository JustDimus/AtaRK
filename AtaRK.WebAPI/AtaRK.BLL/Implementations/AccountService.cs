using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models.Entities;
using AtaRK.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IEncryptionService _encryptionService;

        private readonly IAuthorizationService _authorizationService;

        private readonly IRepository<Account> _accountRepository;

        public AccountService(
            IEncryptionService encryptionService,
            IAuthorizationService authorizationService,
            IRepository<Account> accountRepository)
        {
            this._encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            this._accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ServiceResult<AccountInformation>> GetAuthorizedAccountInformationAsync()
        {
            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unauthorized operation");
                return ServiceResult<AccountInformation>.Instance(false);
            }

            try
            {
                var existingAccount = await this._accountRepository.FirstOrDefaultAsync(i => i.Id == account.Id);

                if (existingAccount == null)
                {
                    this._logger.Error($"Account with such id: '{account.Id}' doesn't exist");
                    return ServiceResult<AccountInformation>.Instance(false);
                }

                var result = new AccountInformation()
                {
                    FirstName = existingAccount.FirstName,
                    SecondName = existingAccount.SecondName
                };

                return ServiceResult<AccountInformation>.FromResult(result);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return ServiceResult<AccountInformation>.Instance(false);
            }
        }

        public async Task<ServiceResult<AuthorizationIdentifier>> GetAccountByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                this._logger.Error($"{nameof(email)} is invalid: '{email}'");
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }

            try
            {
                var account = await this._accountRepository.FirstOrDefaultAsync(i => i.Email == email);

                if (account == null)
                {
                    _logger.Error($"Account with that email: '{email}' doesn't exist");
                    return ServiceResult<AuthorizationIdentifier>.Instance(false);
                }

                AuthorizationIdentifier authInfo = new AuthorizationIdentifier()
                {
                    Id = account.Id,
                    Email = account.Email
                };

                return authInfo;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }
        }

        public async Task<ServiceResult<AuthorizationIdentifier>> LoginAsync(AccountCredentials credentials)
        {
            if (credentials == null)
            {
                this._logger.Error($"{nameof(credentials)} is null");
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }

            try
            {
                var account = await this._accountRepository
                    .FirstOrDefaultAsync(a => a.Email == credentials.Email
                        && a.Password == this._encryptionService.Hash(credentials.Password));

                if (account == null)
                {
                    return ServiceResult<AuthorizationIdentifier>.Instance(false);
                }

                var authorizationInfo = new AuthorizationIdentifier()
                {
                    Email = account.Email,
                    Id = account.Id
                };

                return authorizationInfo;
            }
            catch (Exception ex)
            {
                this._logger.Info(ex, ex.InnerException.Message);
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }
        }

        public async Task<ServiceResult<AuthorizationIdentifier>> RegisterAsync(AccountRegistrationData registrationData)
        {
            if (registrationData == null
                || registrationData.Credentials == null
                || registrationData.Information == null)
            {
                this._logger.Error($"Invalid data");
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }

            Account account = new Account()
            {
                Email = registrationData.Credentials.Email,
                Password = this._encryptionService.Hash(registrationData.Credentials.Password),
                RegistrationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                FirstName = registrationData.Information.FirstName,
                SecondName = registrationData.Information.SecondName
            };

            try
            {
                await this._accountRepository.CreateAsync(account);

                await this._accountRepository.SaveAsync();

                var authorizationInfo = new AuthorizationIdentifier()
                {
                    Id = account.Id,
                    Email = account.Email
                };

                return authorizationInfo;
            }
            catch (Exception ex)
            {
                this._logger.Info(ex, ex.InnerException.Message);
                return ServiceResult<AuthorizationIdentifier>.Instance(false);
            }
        }

        public async Task<ServiceResult> ChangeAccountAsync(AccountInformation accountInfo)
        {
            if (accountInfo == null)
            {
                this._logger.Error($"{nameof(accountInfo)} is null");
                return false;
            }

            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return false;
            }

            try
            {
                var currentAccount = await this._accountRepository.FirstOrDefaultAsync(i => i.Id == account.Id);

                if (currentAccount == null)
                {
                    this._logger.Error($"Account with id '{account.Id}' doesn't exist");
                    return false;
                }

                currentAccount.FirstName = accountInfo.FirstName;
                currentAccount.SecondName = accountInfo.SecondName;

                await this._accountRepository.UpdateAsync(currentAccount);

                await this._accountRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult> ChangePasswordAsync(PasswordChange passwordChange)
        {
            if (passwordChange == null
                || string.IsNullOrWhiteSpace(passwordChange.NewPassword)
                || string.IsNullOrWhiteSpace(passwordChange.OldPassword))
            {
                this._logger.Error($"Invalid data");
                return false;
            }

            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return false;
            }

            try
            {
                var currentAccount = await this._accountRepository.FirstOrDefaultAsync(i => i.Id == account.Id);

                if (currentAccount == null)
                {
                    this._logger.Error($"Account with id '{account.Id}' doesn't exist");
                    return false;
                }

                if (!this._encryptionService.CompareWithHash(
                    passwordChange.OldPassword,
                    currentAccount.Password))
                {
                    this._logger.Error("Old password mismatch current one");
                    return false;
                }

                currentAccount.Password = this._encryptionService.Hash(passwordChange.NewPassword);

                await this._accountRepository.UpdateAsync(currentAccount);

                await this._accountRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }
    }
}
