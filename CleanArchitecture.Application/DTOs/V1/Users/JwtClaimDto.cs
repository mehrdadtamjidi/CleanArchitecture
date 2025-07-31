using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.DTOs.V1.Users
{
    public class JwtClaimDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SecurityStamp { get; set; }
        public List<string> Role { get; set; }
        public bool Remember { get; set; }
    }
}
