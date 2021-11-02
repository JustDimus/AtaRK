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
            //this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this._encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

            this._accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<ServiceResult<string>> LoginAsync(AccountCredentials credentials)
        {
            if (credentials == null)
            {
                this._logger.Error($"{nameof(credentials)} is null");
                return ServiceResult<string>.Instance(false);
            }

            try
            {
                var account = await this._accountRepository
                    .FirstOrDefaultAsync(a => a.Email == credentials.Email
                        && a.Password == this._encryptionService.Hash(credentials.Password));

                if (account == null)
                {
                    return ServiceResult<string>.Instance(false);
                }

                var encryptedId = this._authorizationService.GetAccountIdentifier(account.Id);

                return encryptedId;
            }
            catch (Exception ex)
            {
                this._logger.Info(ex, ex.InnerException.Message);
                return ServiceResult<string>.Instance(false);
            }
        }

        public async Task<ServiceResult<string>> RegisterAsync(AccountRegistrationData registrationData)
        {
            if (registrationData == null
                || registrationData.Credentials == null
                || registrationData.Information == null)
            {
                this._logger.Error($"Invalid data");
                return ServiceResult<string>.Instance(false);
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

                var encryptedId = this._authorizationService.GetAccountIdentifier(account.Id);

                return encryptedId;
            }
            catch (Exception ex)
            {
                this._logger.Info(ex, ex.InnerException.Message);
                return ServiceResult<string>.Instance(false);
            }
        }
    }
}
