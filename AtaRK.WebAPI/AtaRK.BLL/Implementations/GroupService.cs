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
    public class GroupService : IGroupService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IAuthorizationService _authorizationService;

        private readonly IRepository<DeviceGroup> _groupRepository;

        private readonly IRepository<AccountDeviceGroup> _accountGroupRepository;

        public GroupService(
            IAuthorizationService authorizationService,
            IRepository<AccountDeviceGroup> accountGroupRepository,
            IRepository<DeviceGroup> groupRepository)
        {
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

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
                this._logger.Error("Unauthorized operation");
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
    }
}
