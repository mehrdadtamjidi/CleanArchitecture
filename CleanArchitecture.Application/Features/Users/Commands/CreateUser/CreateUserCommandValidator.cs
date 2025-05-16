using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.FirstName)
                   .NotEmpty().WithMessage("this field is required")
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 50");

            RuleFor(u => u.LastName)
                   .NotEmpty().WithMessage("this field is required")
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 50");
        }
    }
}
