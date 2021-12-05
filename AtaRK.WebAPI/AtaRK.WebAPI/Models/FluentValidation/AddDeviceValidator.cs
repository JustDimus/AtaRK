using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class AddDeviceValidator : AbstractValidator<AddDeviceVM>
    {
        public AddDeviceValidator()
        {
            RuleFor(i => i.GroupId).NotEmpty();
            RuleFor(i => i.DeviceType).NotEmpty();
            RuleFor(i => i.DeviceCode).NotEmpty();
        }
    }
}
