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

        public DesignTimeGroupService(
            IDataManager dataManager)
        {
            this._dataManager = dataManager;
        }

        public IObservable<GroupInformation> GroupInfoObservable => this._groupInfoSubject.AsObservable();

        public Task<bool> GetGroupInfo(string groupId)
        {
            var groupInfo = new GroupInformation()
            {
                GroupName = "Group Name",
                UserRole = UserRole.UserRole.CoOwner
            };

            this._groupInfoSubject.OnNext(groupInfo);

            return Task.FromResult(true);
        }

        public Task<RequestContext<ListData<GroupInfo>>> GetGroups()
        {
            return this._dataManager.GetGroupsInfo();
        }
    }
}
