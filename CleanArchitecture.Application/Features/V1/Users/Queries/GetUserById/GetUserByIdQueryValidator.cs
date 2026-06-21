using FluentValidation;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
        }
    }
}
