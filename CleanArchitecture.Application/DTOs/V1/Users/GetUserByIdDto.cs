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
    }
}
