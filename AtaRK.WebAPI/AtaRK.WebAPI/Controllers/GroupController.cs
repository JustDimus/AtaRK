using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.Utility.Json;
using AtaRK.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAuthorizationService = AtaRK.BLL.Interfaces.IAuthorizationService;

namespace AtaRK.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IJsonEncryptionService _encryptionService;
        private readonly IAuthorizationService _authorizationService;

        public GroupController(
            IGroupService groupService,
            IJsonEncryptionService encryptionService,
            IAuthorizationService authorizationService)
        {
            this._groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
            this._encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            this._authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateGroupVM group)
        {
            var groupInfo = new GroupCreationInfo()
            {
                GroupName = group.Name
            };

            var serviceResult = await this._groupService.CreateGroupAsync(groupInfo);

            if (serviceResult)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] SingleBodyParameter parameter)
        {
            var groupInfo = this.DeserializeGroupInfo(parameter.Body);

            if (groupInfo == null)
            {
                return BadRequest();
            }

            var serviceResult = await this._groupService.DeleteGroupAsync(groupInfo);

            if (serviceResult)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        [Route("changeuserrole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleVM changeUserRoleContext)
        {
            var groupInfo = this.DeserializeGroupInfo(changeUserRoleContext.GroupId);
            var accountInfo = this._authorizationService.GetAuthorizedAccount(changeUserRoleContext.AccountId);

            if (groupInfo != null && accountInfo != null)
            {
                var serviceResult = await this._groupService.ChangeUserRole(groupInfo, accountInfo, changeUserRoleContext.NewRole);

                if (serviceResult)
                {
                    return Ok();
                }
                else
                {
                    return Conflict();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("invite")]
        public async Task<IActionResult> Invite([FromBody] CreateInviteVM invite)
        {
            var groupInfo = this.DeserializeGroupInfo(invite.GroupId);
            var accountInfo = this._authorizationService.GetAuthorizedAccount(invite.AccountId);

            if (groupInfo != null && accountInfo != null)
            {
                var serviceResult = await this._groupService.InviteAccountToGroup(groupInfo, accountInfo);

                if (serviceResult)
                {
                    return Ok();
                }
                else
                {
                    return Conflict();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("operateinvite")]
        public async Task<IActionResult> OperateInvite([FromBody] InviteVM invite)
        {
            var inviteInfo = this._encryptionService.Decrypt<InviteIdentifier>(invite.InviteId);

            var serviceResult = await this._groupService.OperateInvitation(inviteInfo, invite.DoAccept.Value);

            if (serviceResult)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpGet]
        [Route("invitelist")]
        public async Task<IActionResult> GetInviteList()
        {
            var serviceResult = await this._groupService.GetAccountInvites();

            if (serviceResult)
            {
                var inviteListResult = serviceResult.Result
                    .Select(i => new InviteListElementVM()
                    {
                        GroupName = i.GroupName,
                        InviteId = this._encryptionService.Encrypt(i)
                    })
                    .ToList();

                return new JsonResult(new ListVM<InviteListElementVM>()
                {
                    Elements = inviteListResult
                });
            }

            return Conflict();
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            var serviceResult = await this._groupService.GetAccountGroupList();

            if (serviceResult)
            {
                var groupListResult = serviceResult.Result
                    .Select(i => new GroupListElementVM()
                    {
                        Id = this.SerializeGroupInfo(i),
                        Name = i.Name
                    })
                    .ToList();

                return new JsonResult(new ListVM<GroupListElementVM>()
                {
                    Elements = groupListResult
                });
            }

            return Conflict();
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateGroupVM group)
        {
            var groupInfo = this.DeserializeGroupInfo(group.GroupId);

            if (groupInfo == null)
            {
                return BadRequest();
            }

            var serviceResult = await this._groupService.UpdateGroupAsync(groupInfo, group.Name);

            if (serviceResult)
            {
                return Ok();
            }

            return Conflict();
        }

        [HttpPost]
        [Route("groupinfo")]
        public async Task<IActionResult> GetGroupInfo([FromBody] SingleBodyParameter groupId)
        {
            var groupInfo = this.DeserializeGroupInfo(groupId.Body);

            if (groupId != null)
            {
                var serviceResult = await this._groupService.GetGroupInformation(groupInfo);

                if (serviceResult)
                {
                    var result = new GroupInformationVM()
                    {
                        GroupName = serviceResult.Result.GroupName,
                        UserRole = serviceResult.Result.UserRole
                    };

                    return new JsonResult(result);
                }
            }

            return BadRequest();
        }

        private string SerializeGroupInfo(GroupIdentifier groupInfo)
        {
            return this._encryptionService.Encrypt(groupInfo);
        }

        private GroupIdentifier DeserializeGroupInfo(string groupInfo)
        {
            return this._encryptionService.Decrypt<GroupIdentifier>(groupInfo);
        }
    }
}
