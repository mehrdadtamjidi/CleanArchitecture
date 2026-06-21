using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class GetUserByIdInputDto
    {
        public int Id { get; set; }
    }

    public class GetUserByIdOutputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public static GetUserByIdOutputDto FromEntity(User user) => new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName
        };
    }
}
