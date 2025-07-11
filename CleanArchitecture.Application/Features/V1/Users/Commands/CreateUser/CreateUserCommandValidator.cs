using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 100 characters.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters.");

            RuleFor(u => u.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password must be less than 100 characters.");

            RuleFor(u => u.Gender)
                .IsInEnum().WithMessage("{PropertyName} is invalid.");
        }
    }
}
