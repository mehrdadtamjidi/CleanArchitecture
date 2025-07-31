using CleanArchitecture.Application.Features.V1.Users.Queries.LoginUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Features.V1.Users.Queries.GetUsers
{
    public class GetUsersQueryValidator: AbstractValidator<GetUsersQuery>
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
