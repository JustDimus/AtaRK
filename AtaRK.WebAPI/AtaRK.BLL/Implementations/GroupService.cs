using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Core.Models;
using AtaRK.Core.Models.Entities;
using AtaRK.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IRepository<Invite> _inviteRepository;

        private readonly IRepository<AccountDeviceGroup> _accountGroupRepository;

        public GroupService(
            IAuthorizationService authorizationService,
            IAccountService accountService,
            IRepository<AccountDeviceGroup> accountGroupRepository,
            IRepository<DeviceGroup> groupRepository,
            IRepository<Invite> inviteRepository)
        {
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            this._accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

            this._accountGroupRepository = accountGroupRepository ?? throw new ArgumentNullException(nameof(accountGroupRepository));
            this._groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            this._inviteRepository = inviteRepository ?? throw new ArgumentNullException(nameof(inviteRepository));

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
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult> InviteAccountToGroup(GroupIdentifier groupInfo, AuthorizationIdentifier accountInfo)
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

                var invite = new Invite()
                {
                    CreatorId = account.Id,
                    GroupId = groupInfo.Id,
                    InvitedId = accountInfo.Id,
                    CreationDate = DateTime.UtcNow
                };

                await this._inviteRepository.CreateAsync(invite);

                await this._accountGroupRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult> OperateInvitation(InviteIdentifier invite, bool doAccept)
        {
            if (invite == null)
            {
                this._logger.Error($"{nameof(invite)} is null");
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
                var existingInvite = await this._inviteRepository
                    .FirstOrDefaultAsync(i => i.GroupId == invite.GroupId && i.InvitedId == invite.InvitedId);

                if (existingInvite == null)
                {
                    this._logger.Error($"Invite account: '{invite.InvitedId}' to group: '{invite.GroupId}' doesn't exist");
                    return false;
                }

                if (doAccept)
                {
                    bool creatorStillExist = await this._accountGroupRepository
                        .AnyAsync(i => i.AccountId == invite.CreatorId
                            && i.GroupId == invite.GroupId
                            && (i.Role == MemberRole.CoOwner || i.Role == MemberRole.Owner));

                    if (!creatorStillExist)
                    {
                        this._logger.Error($"Creator: '{invite.CreatorId}' doesn't exist in the group: '{invite.GroupId}'");
                        return false;
                    }

                    var accountDevice = new AccountDeviceGroup()
                    {
                        AccountId = invite.InvitedId,
                        GroupId = invite.GroupId,
                        Role = MemberRole.Spectator
                    };

                    await this._accountGroupRepository.CreateAsync(accountDevice);
                }

                await this._inviteRepository.DeleteAsync(existingInvite);

                await this._inviteRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult> DeleteGroupAsync(GroupIdentifier groupInfo)
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
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult<GroupInfo>> GetGroupInformation(GroupIdentifier groupInfo)
        {
            if (groupInfo == null)
            {
                this._logger.Error($"{nameof(groupInfo)} is null");
                return ServiceResult<GroupInfo>.Instance(false);
            }

            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return ServiceResult<GroupInfo>.Instance(false);
            }

            try
            {
                var group = await this._groupRepository.FirstOrDefaultAsync(i => i.Id == groupInfo.Id);

                if (group == null)
                {
                    this._logger.Error($"Group with id: '{groupInfo.Id}' doesn't exist");
                    return ServiceResult<GroupInfo>.Instance(false); ;
                }

                var accountGroupInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.AccountId == account.Id
                        && i.GroupId == groupInfo.Id);

                if (accountGroupInfo == null)
                {
                    this._logger.Error($"User can't be found in group");
                    return ServiceResult<GroupInfo>.Instance(false);
                }

                return ServiceResult<GroupInfo>.FromResult(new GroupInfo()
                {
                    GroupName = groupInfo.Name,
                    UserRole = this.GetGroupRole(accountGroupInfo.Role)
                });
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return ServiceResult<GroupInfo>.Instance(false);
            }
        }

        public async Task<ServiceResult> UpdateGroupAsync(GroupIdentifier groupInfo, string newName)
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
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return false;
            }
        }

        public async Task<ServiceResult<List<GroupIdentifier>>> GetAccountGroupList()
        {
            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return ServiceResult<List<GroupIdentifier>>.Instance(false);
            }

            try
            {
                var groups = await this._accountGroupRepository
                    .SelectAsync(
                        i => i.AccountId == account.Id,
                        i => new GroupIdentifier()
                        {
                            Id = i.Group.Id,
                            Name = i.Group.GroupName
                        });

                var result = new List<GroupIdentifier>(groups);

                return result;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return ServiceResult<List<GroupIdentifier>>.Instance(false);
            }
        }

        public async Task<ServiceResult<List<InviteIdentifier>>> GetAccountInvites()
        {
            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return ServiceResult<List<InviteIdentifier>>.Instance(false);
            }

            try
            {
                var invites = await this._inviteRepository
                    .SelectAsync(
                        i => i.InvitedId == account.Id,
                        i => new InviteIdentifier()
                        {
                            GroupName = i.Group.GroupName,
                            GroupId = i.GroupId,
                            CreatorId = i.CreatorId,
                            InvitedId = i.InvitedId
                        });

                var result = new List<InviteIdentifier>(invites);

                return result;
            }
            catch (Exception ex)
            {
                this._logger.Error(ex, ex.InnerException.Message);
                return ServiceResult<List<InviteIdentifier>>.Instance(false);
            }
        }

        public async Task<ServiceResult> ChangeUserRole(GroupIdentifier groupId, AuthorizationIdentifier accountId, string newRole)
        {
            var account = this._authorizationService.GetAuthorizedAccountFromCurrentContext();

            if (account == null)
            {
                this._logger.Error("Unable to get authorized account");
                return false;
            }

            try
            {
                var currentUserGroupAccountInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.AccountId == account.Id
                        && i.GroupId == groupId.Id);

                if (currentUserGroupAccountInfo == null
                    || (currentUserGroupAccountInfo.Role != MemberRole.Owner && currentUserGroupAccountInfo.Role != MemberRole.CoOwner))
                {
                    return false;
                }

                var groupAccountInfo = await this._accountGroupRepository
                    .FirstOrDefaultAsync(i => i.AccountId == accountId.Id
                        && i.GroupId == groupId.Id);

                if (groupAccountInfo == null
                    || (groupAccountInfo.Role >= currentUserGroupAccountInfo.Role))
                {
                    return false;
                }

                var assignedRole = this.GetGroupRoleFromString(newRole);

                if (assignedRole == MemberRole.Undefined)
                {
                    return false;
                }

                groupAccountInfo.Role = assignedRole;

                await this._accountGroupRepository.UpdateAsync(groupAccountInfo);

                if (assignedRole == MemberRole.Owner)
                {
                    currentUserGroupAccountInfo.Role = MemberRole.CoOwner;
                    await this._accountGroupRepository.UpdateAsync(currentUserGroupAccountInfo);
                }

                await this._accountGroupRepository.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private MemberRole GetGroupRoleFromString(string memberRole)
        {
            return memberRole switch
            {
                "Owner" => MemberRole.Owner,
                "CoOwner" => MemberRole.CoOwner,
                "Spectator" => MemberRole.Spectator,
                "User" => MemberRole.User,
                _ => MemberRole.Undefined
            };
        }

        private string GetGroupRole(MemberRole memberRole)
        {
            return memberRole switch
            {
                MemberRole.Owner => "Owner",
                MemberRole.CoOwner => "CoOwner",
                MemberRole.Spectator => "Spectator",
                MemberRole.User => "User",
                _ => "Undefined"
            };
        }
    }
}
