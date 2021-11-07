using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models
{
    public class AccountInformationVM
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("second_name")]
        public string SecondName { get; set; }
    }
}
