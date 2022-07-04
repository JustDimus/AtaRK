using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.Models.MvcModels
{
    public class RegistrationModel : LoginModel
    {
        [JsonProperty("confirm_password")]
        public string ConfirmPassword { get; set; }
    }
}
