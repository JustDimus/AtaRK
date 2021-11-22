using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.BLL.Interfaces
{
    public interface IGroupService
    {
        Task<ServiceResult> CreateGroupAsync(GroupCreationInfo groupInfo);

        Task<ServiceResult> UpdateGroupAsync(GroupIdentifier groupInfo, string newName);

        Task<ServiceResult> DeleteGroupAsync(GroupIdentifier groupInfo);

        Task<ServiceResult> InviteAccountToGroup(GroupIdentifier groupInfo, AuthorizationIdentifier accountInfo);

        Task<ServiceResult<List<GroupIdentifier>>> GetAccountGroupList();

        Task<ServiceResult<List<InviteIdentifier>>> GetAccountInvites();

        Task<ServiceResult> OperateInvitation(InviteIdentifier invite, bool doAccept);

        Task<ServiceResult<GroupInfo>> GetGroupInformation(GroupIdentifier group);
    }
}
