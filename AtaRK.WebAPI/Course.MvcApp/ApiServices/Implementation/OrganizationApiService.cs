using Course.MvcApp.Models.MvcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Course.MvcApp.ApiServices.Implementation
{
    public class OrganizationApiService : BaseApiService, IOrganizationApiService
    {
        private readonly IDeviceApiService _deviceApiService;

        public OrganizationApiService(
            IHttpClientFactory httpClientFactory,
            IDeviceApiService deviceApiService)
            : base(httpClientFactory)
        {
            _deviceApiService = deviceApiService;
        }

        public async Task<bool> CreateNewOrganization(OrganizationModel organizationModel)
        {
            return await this.SendRequestWithoutResponseAsync("group/create", RequestMethod.POST, organizationModel);
        }

        public async Task<IEnumerable<InviteModel>> GetCurrentUserInvitations()
        {
            var requestResult = await this.SendRequestAsync<ListModel<InviteModel>>("group/invitelist", RequestMethod.GET);

            if (requestResult != null)
            {
                return requestResult.Elements;
            }

            return Enumerable.Empty<InviteModel>();
        }

        public async Task<IEnumerable<OrganizationModel>> GetCurrentUserOrganizations()
        {
            var requestResult = await this.SendRequestAsync<ListModel<OrganizationModel>>("group/list", RequestMethod.GET);

            if (requestResult != null)
            {
                return requestResult.Elements;
            }

            return Enumerable.Empty<OrganizationModel>();
        }

        public async Task<OrganizationShowModel> GetOrganizationInfo(string organizationId)
        {
            var result = await this.SendRequestAsync<OrganizationShowModel>("group/groupinfo", RequestMethod.POST, new { body = organizationId });

            if (result != null)
            {
                result.OrganizationId = organizationId;
                var groupDevices = await this._deviceApiService.GetDevicesInOrganization(organizationId);

                result.Devices = groupDevices.Elements;
            }

            return result;
        }

        public async Task<bool> InviteUser(string groupId, string userEmail)
        {
            var accountIdResult = await this.SendRequestAsync<AccountIdentifierModel>(
                "account/getid",
                RequestMethod.POST,
                new
                {
                    body = userEmail
                });

            if (accountIdResult == null)
            {
                return false;
            }

            var result = await this.SendRequestWithoutResponseAsync(
               "group/invite",
               RequestMethod.POST,
               new
               {
                   group_id = groupId,
                   account_id = accountIdResult.AccountId
               });

            return result;
        }

        public async Task<bool> OperateInvite(string inviteId, bool doAccept)
        {
            var result = await this.SendRequestWithoutResponseAsync(
                "group/operateinvite",
                RequestMethod.POST,
                new
                {
                    group_name = string.Empty,
                    invite_id = inviteId,
                    accept = doAccept
                });

            return result;
        }
    }
}
