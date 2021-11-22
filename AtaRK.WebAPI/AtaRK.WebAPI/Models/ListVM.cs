using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class ListVM<TElement>
    {
        [JsonProperty("list")]
        public List<TElement> Elements { get; set; }
    }
}
