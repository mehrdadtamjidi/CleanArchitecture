using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class CreateUserInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
    }

    public class CreateUserOutputDto
    {
        public int Id { get; set; }
    }
}
