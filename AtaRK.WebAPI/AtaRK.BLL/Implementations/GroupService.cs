using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models;
using AtaRK.Core.Models.Entities;
using AtaRK.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAuthorizationService _authorizationService;

        private readonly IAccountService _accountService;

        private readonly IRepository<DeviceGroup> _groupRepository;

        private readonly IRepository<AccountDeviceGroup> _accountGroupRepository;

        public GroupService(
            IAuthorizationService authorizationService,
            IAccountService accountService,
            IRepository<AccountDeviceGroup> accountGroupRepository,
            IRepository<DeviceGroup> groupRepository)
        {
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this._accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

            this._accountGroupRepository = accountGroupRepository ?? throw new ArgumentNullException(nameof(accountGroupRepository));
            this._groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        
        
        }

        public async Task<ServiceResult> CreateGroupAsync(GroupCreationInfo groupInfo)
        {
            if (groupInfo == null)
            {
                this._logger.Error($"{nameof(groupInfo)} is null");
                return false;
            }

            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return false;
            }

            DeviceGroup group = new DeviceGroup()
            {
                GroupName = groupInfo.GroupName
            };

            try
            {
                await this._groupRepository.CreateAsync(group);

                AccountDeviceGroup accountGroup = new AccountDeviceGroup()
                {
                    AccountId = account.Id,
                    GroupId = group.Id,
                    Role = Core.Models.MemberRole.Owner
                };

                await this._accountGroupRepository.CreateAsync(accountGroup);

                await this._accountGroupRepository.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                this._logger.Error(ex);
                return false;
            }
        }

        public async Task<ServiceResult> AddAccountGroup(GroupInfo groupInfo, AuthorizationInfo accountInfo)
        {
            if (groupInfo == null)
            {
                this._logger.Error($"{nameof(groupInfo)} is null");
                return false;
            }

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
                bool isGroupExist = await this._groupRepository.AnyAsync(i => i.Id == groupInfo.Id);

                if (!isGroupExist)
                {
                    this._logger.Error($"Group with id: '{groupInfo.Id}' doesn't exist");
                    return false;
                }

                bool canAddAccount = await this._accountGroupRepository
                    .AnyAsync(i => i.AccountId == account.Id
                        && i.GroupId == groupInfo.Id
                        && (i.Role == MemberRole.CoOwner || i.Role == MemberRole.Owner));

                if (!canAddAccount)
                {
                    this._logger.Error($"Account can't change group");
                    return false;
                }

                AccountDeviceGroup accountGroup = new AccountDeviceGroup()
                {
                    AccountId = accountInfo.Id,
                    GroupId = groupInfo.Id,
                    Role = MemberRole.Spectator
                };

                await this._accountGroupRepository.CreateAsync(accountGroup);

                await this._accountGroupRepository.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                this._logger.Error(ex);
                return false;
            }
        }

        public async Task<ServiceResult> DeleteGroupAsync(GroupInfo groupInfo)
        {
            if (groupInfo == null)
            {
                this._logger.Error($"{nameof(groupInfo)} is null");
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
                bool groupExist = await this._groupRepository.AnyAsync(i => i.Id == groupInfo.Id);

                if (!groupExist)
                {
                    this._logger.Error($"Group with id: '{groupInfo.Id}' doesn't exist");
                    return false;
                }

                bool canDeleteGroup = await this._accountGroupRepository
                    .AnyAsync(i => i.GroupId == groupInfo.Id 
                        && i.AccountId == account.Id 
                        && i.Role == Core.Models.MemberRole.Owner);

                if (!canDeleteGroup)
                {
                    this._logger.Error("Account has no permissions to delete group");
                    return false;
                }

                await this._groupRepository.DeleteAsync(i => i.Id == groupInfo.Id);

                await this._groupRepository.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                this._logger.Error(ex);
                return false;
            }
        }

        public async Task<ServiceResult> ChangeGroupName(GroupInfo groupInfo, string newName)
        {
            if (groupInfo == null)
            {
                this._logger.Error($"{nameof(groupInfo)} is null");
                return false;
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                this._logger.Error($"New group name is invalid: '{newName}'");
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
                var group = await this._groupRepository.FirstOrDefaultAsync(i => i.Id == groupInfo.Id);

                if (group == null)
                {
                    this._logger.Error($"Group with id: '{groupInfo.Id}' doesn't exist");
                    return false;
                }

                bool canModify = await this._accountGroupRepository
                    .AnyAsync(i => i.AccountId == account.Id
                        && i.GroupId == groupInfo.Id
                        && (i.Role == MemberRole.CoOwner || i.Role == MemberRole.Owner));

                if (!canModify)
                {
                    this._logger.Error($"Account can't change group");
                    return false;
                }

                group.GroupName = newName;

                await this._groupRepository.UpdateAsync(group);

                await this._groupRepository.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                this._logger.Error(ex);
                return false;
            }
        }
    }
}
