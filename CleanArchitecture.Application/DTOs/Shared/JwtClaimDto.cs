namespace CleanArchitecture.Application.DTOs.Shared
{
    public class JwtClaimDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SecurityStamp { get; set; }
        public List<string> Role { get; set; }
    }
}
