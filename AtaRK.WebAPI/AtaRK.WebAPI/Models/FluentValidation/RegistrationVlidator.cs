using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Models.FluentValidation
{
    public class RegistrationVlidator : AbstractValidator<RegistrationVM>
    {
        public RegistrationVlidator()
        {
            RuleFor(i => i.Email)
                .NotEmpty();
            RuleFor(i => i.Password)
                .NotEmpty();
            RuleFor(i => i.ConfirmPassword)
                .NotEmpty();
            RuleFor(i => i.SecondName)
                .NotEmpty();

            RuleFor(i => i.Password).Equal(i => i.ConfirmPassword);
        }
    }
}
