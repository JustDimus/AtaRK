using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class InviteModel
    {
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        [JsonProperty("invite_id")]
        public string InviteId { get; set; }
    }
}
