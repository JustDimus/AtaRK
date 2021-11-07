using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class UpdateGroupVM
    {
        [JsonProperty("id")]
        public string GroupId { get; set; }

        [JsonProperty("new_name")]
        public string Name { get; set; }
    }
}
