using AtaRK.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DataManager
{
    public interface IDataManager
    {
        Task<RequestContext<ListData<GroupInfo>>> GetGroupsInfo();

        Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId);
    }
}
