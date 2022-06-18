using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class ChangeUserRoleVM
    {
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("new_role")]
        public string NewRole { get; set; }
    }
}
