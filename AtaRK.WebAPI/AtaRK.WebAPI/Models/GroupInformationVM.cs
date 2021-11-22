using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class GroupInformationVM
    {
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [JsonProperty("user_role")]
        public string UserRole { get; set; }
    }
}
