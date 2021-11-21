using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class UpdateDeviceSettingsValidator : AbstractValidator<UpdateDeviceSettingsVM>
    {
        public UpdateDeviceSettingsValidator()
        {
            RuleFor(i => i.DeviceId).NotEmpty();
            RuleForEach(i => i.Settings).SetValidator(new DeviceSettingValidator());
        }
    }
}
