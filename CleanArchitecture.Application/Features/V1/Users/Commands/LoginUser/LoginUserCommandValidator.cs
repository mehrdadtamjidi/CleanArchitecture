using FluentValidation;

namespace CleanArchitecture.Application.Features.V1.Users.Commands.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty()
                .WithMessage($"{nameof(LoginUserCommand.UserName)} is required.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
