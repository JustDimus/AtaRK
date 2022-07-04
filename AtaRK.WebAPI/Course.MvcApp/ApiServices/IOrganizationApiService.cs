using Course.MvcApp.Models.MvcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices
{
    public interface IOrganizationApiService
    {
        Task<IEnumerable<OrganizationModel>> GetCurrentUserOrganizations();

        Task<bool> CreateNewOrganization(OrganizationModel organizationModel);

        Task<OrganizationShowModel> GetOrganizationInfo(string organizationId);

        Task<IEnumerable<InviteModel>> GetCurrentUserInvitations();

        Task<bool> OperateInvite(string inviteId, bool doAccept);

        Task<bool> InviteUser(string groupId, string userEmail);
    }
}
