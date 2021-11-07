using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class CreateInviteValidator : AbstractValidator<CreateInviteVM>
    {
        public CreateInviteValidator()
        {
            RuleFor(i => i.AccountId).NotEmpty();
            RuleFor(i => i.GroupId).NotEmpty();
        }
    }
}
