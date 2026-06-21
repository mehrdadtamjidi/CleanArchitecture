using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common
{
    public class SiteSettings
    {
        public JwtConfig JwtConfig { get; set; }
        public RedisSettings Redis { get; set; }
    }

    public class RedisSettings
    {
        public string[] Nodes { get; set; } = [];
        public string InstanceName { get; set; } = string.Empty;
        public int DefaultExpirationMinutes { get; set; } = 60;
    }


    public class JwtConfig
    {
        public string SecretKey { get; set; }
        public string Encryptkey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
