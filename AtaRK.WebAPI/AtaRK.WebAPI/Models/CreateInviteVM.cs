using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class CreateInviteVM
    {
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }
    }
}
