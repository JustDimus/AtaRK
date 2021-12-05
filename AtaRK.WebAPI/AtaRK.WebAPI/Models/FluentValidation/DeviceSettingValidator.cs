using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class DeviceSettingValidator : AbstractValidator<DeviceSettingVM>
    {
        public DeviceSettingValidator()
        {
            RuleFor(i => i.Setting).NotEmpty();
        }
    }
}
