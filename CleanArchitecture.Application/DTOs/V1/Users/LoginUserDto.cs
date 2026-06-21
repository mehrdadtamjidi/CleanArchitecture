namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class LoginUserInputDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserOutputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
