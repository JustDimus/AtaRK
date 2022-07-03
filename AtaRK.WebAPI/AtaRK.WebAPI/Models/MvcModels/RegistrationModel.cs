﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.MvcModels
{
    public class RegistrationModel : LoginModel
    {
        [JsonProperty("confirm_password")]
        public string ConfirmPassword { get; set; }
    }
}
