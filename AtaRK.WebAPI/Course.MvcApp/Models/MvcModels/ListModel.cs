using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class ListModel<TElement>
    {
        [JsonProperty("list")]
        public List<TElement> Elements { get; set; }
    }
}
