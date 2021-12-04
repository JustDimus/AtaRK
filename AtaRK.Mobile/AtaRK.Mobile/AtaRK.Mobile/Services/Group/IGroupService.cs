using AtaRK.Mobile.Models;
using AtaRK.Mobile.Services.DataManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Group
{
    public interface IGroupService
    {
        IObservable<GroupInformation> GroupInfoObservable { get; }

        Task<bool> GetGroupInfo(string groupId);

        Task<RequestContext<ListData<GroupInfo>>> GetGroups();
    }
}
