using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupVM>
    {
        public CreateGroupValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
        }
    }
}
