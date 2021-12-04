using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.Mobile.Models
{
    public class ListData<TElement>
    {
        [JsonProperty("list")]
        public List<TElement> Elements { get; set; }
    }
}
