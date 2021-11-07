using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class SingleBodyParameter
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
