using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class CreateGroupVM
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
