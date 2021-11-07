using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class InviteValidator : AbstractValidator<InviteVM>
    {
        public InviteValidator()
        {
            RuleFor(i => i.InviteId).NotEmpty();

            RuleFor(i => i.DoAccept).NotNull();
        }
    }
}
