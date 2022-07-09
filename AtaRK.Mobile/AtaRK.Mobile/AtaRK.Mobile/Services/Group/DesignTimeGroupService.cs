using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using AtaRK.Mobile.Services.Group.UserRole;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Group
{
    public class DesignTimeGroupService : IGroupService
    {
        private ReplaySubject<GroupInformation> _groupInfoSubject = new ReplaySubject<GroupInformation>(1);

        private IDataManager _dataManager;
        private IUserRoleManager _roleManager;

        public DesignTimeGroupService(
            IDataManager dataManager,
            IUserRoleManager roleManager)
        {
            this._dataManager = dataManager;
            this._roleManager = roleManager;
        }

        public IObservable<GroupInformation> GroupInfoObservable => this._groupInfoSubject.AsObservable();

        public async Task<bool> GetGroupInfo(string groupId)
        {
            var groupInfo = await this._dataManager.GetGroupInfo(groupId);

            if (groupInfo)
            {
                this._groupInfoSubject.OnNext(new GroupInformation()
                {
                    GroupId = groupId,
                    GroupName = groupInfo.Result.GroupName,
                    UserRole = this._roleManager.GetUserRole(groupInfo.Result.UserRole)
                });
            }

            return groupInfo;
        }

        public Task<RequestContext<ListData<GroupInfo>>> GetGroups()
        {
            return this._dataManager.GetGroupsInfo();
        }
    }
}
