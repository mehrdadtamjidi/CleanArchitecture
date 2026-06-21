namespace CleanArchitecture.Application.Contracts.Infrastructure
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}
