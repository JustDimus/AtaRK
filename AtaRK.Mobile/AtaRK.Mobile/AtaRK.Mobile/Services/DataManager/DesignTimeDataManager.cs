using AtaRK.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.DataManager
{
    public class DesignTimeDataManager : IDataManager
    {
        public Task<RequestContext<ListData<DeviceInfo>>> GetGroupDevices(string groupId)
        {
            var random = new Random();

            return Task.FromResult(new RequestContext<ListData<DeviceInfo>>()
            {
                IsSuccessful = new Random().Next(0, 10) > 1,
                Result = new ListData<DeviceInfo>()
                {
                    Elements = new List<DeviceInfo>
                    {
                        new DeviceInfo()
                        {
                            DeviceType = $"Device-{random.Next(0, 3)}",
                            DeviceCode = $"jgvkdfopgjdiofhsduifgbjklwebfhidfvbnjif",
                            Id = $"{new Guid().ToString()}"
                        }
                    }
                }
            });
        }

        public Task<RequestContext<ListData<GroupInfo>>> GetGroupsInfo()
        {
            var random = new Random();

            return Task.FromResult(new RequestContext<ListData<GroupInfo>>()
            {
                IsSuccessful = new Random().Next(0, 10) > 2,
                Result = new ListData<GroupInfo>()
                {
                    Elements = new List<GroupInfo>
                    {
                        new GroupInfo()
                        {
                            GroupName = $"First group - {random.Next(0, 100)}",
                            GroupId = "groupid1"
                        },
                        new GroupInfo()
                        {
                            GroupName = $"Second group - {random.Next(0, 100)}",
                            GroupId = "groupid2"
                        },
                        new GroupInfo()
                        {
                            GroupName = $"Third group - {random.Next(0, 100)}",
                            GroupId = "groupid3"
                        },
                        new GroupInfo()
                        {
                            GroupName = $"Fourth group - {random.Next(0, 100)}",
                            GroupId = "groupid4"
                        }
                    }
                }
            });
        }
    }
}
