using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupVM>
    {
        public UpdateGroupValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
            RuleFor(i => i.GroupId).NotEmpty();
        }
    }
}
