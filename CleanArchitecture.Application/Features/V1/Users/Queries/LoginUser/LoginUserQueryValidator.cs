using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser
{
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty()
                .WithMessage($"{nameof(LoginUserQuery.UserName)} is required.");

            RuleFor(u => u.PasswordHash)
                .NotEmpty()
                .WithMessage($"{nameof(LoginUserQuery.PasswordHash)} is required.");
        }
    }
}
