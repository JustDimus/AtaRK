using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.BLL.Models
{
    public class InviteIdentifier
    {
        [JsonIgnore]
        public string GroupName { get; set; }

        [JsonProperty("creator_id")]
        public Guid CreatorId { get; set; }

        [JsonProperty("invited_id")]
        public Guid InvitedId { get; set; }

        [JsonProperty("group_id")]
        public Guid GroupId { get; set; }
    }
}
