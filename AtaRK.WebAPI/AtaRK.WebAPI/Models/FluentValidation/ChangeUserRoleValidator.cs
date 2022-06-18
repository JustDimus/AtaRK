using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleVM>
    {
        public ChangeUserRoleValidator()
        {
            RuleFor(i => i.AccountId).NotEmpty();
            RuleFor(i => i.GroupId).NotEmpty();
            RuleFor(i => i.NewRole).Must(i =>
            {
                if (i == "CoOwner" || i == "Owner" || i == "Spectator" || i == "User")
                {
                    return true;
                }

                return false;
            });
        }
    }
}
