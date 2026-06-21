using FluentValidation;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(u => u.Page)
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GetUsersQuery.Page)} must be zero or greater.");

            RuleFor(u => u.PerPage)
                .GreaterThan(0)
                .WithMessage($"{nameof(GetUsersQuery.PerPage)} must be greater than zero.");
        }
    }
}
