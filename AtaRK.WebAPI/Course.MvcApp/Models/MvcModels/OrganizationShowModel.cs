using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class OrganizationShowModel
    {
        [JsonIgnore]
        public string OrganizationId { get; set; }

        [JsonProperty("group_name")]
        public string Name { get; set; }

        [JsonProperty("user_role")]
        public string UserRole { get; set; }

        [JsonProperty("devices")]
        public IEnumerable<DeviceModel> Devices { get; set; }

        [JsonIgnore]
        public bool IsAdmin => UserRole == "Owner" || UserRole == "CoOwner";
    }
}
