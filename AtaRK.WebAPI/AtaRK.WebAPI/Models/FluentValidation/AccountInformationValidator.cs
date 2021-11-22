using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class AccountInformationValidator : AbstractValidator<AccountInformationVM>
    {
        public AccountInformationValidator()
        {
            RuleFor(i => i.FirstName).NotEmpty();
            RuleFor(i => i.SecondName).NotEmpty();
        }
    }
}
