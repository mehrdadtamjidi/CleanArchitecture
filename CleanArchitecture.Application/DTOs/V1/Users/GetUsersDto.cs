using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class GetUsersInputDto
    {
        public int Page { get; set; } = 0;
        public int PerPage { get; set; } = 10;
    }

    public class GetUsersOutputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public static GetUsersOutputDto FromEntity(User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName
        };
    }
}
