using Microgroove.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Microgroove.Application.Validators
{
    public class PersonDtoValidator : AbstractValidator<PersonDto>
    {
        public PersonDtoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("FirstName is required.");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("LastName is required.");
        }
    }
}
