using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace CleanArchitecture.Application.Common
{
    public static class IdentityExtensions
    {
        public static string? FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        public static string? FindFirstValue(this IIdentity identity, string claimType)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity?.FindFirstValue(claimType);
        }

        public static string? GetUserId(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public static int? GetBusinessId(this ClaimsPrincipal claims)
        {
            if (claims != null)
            {
                var result = claims.FindFirst("BusinessId").Value;
                if (!string.IsNullOrEmpty(result))
                    return result?.ToInt();
            }
            return default;
        }
        public static bool? IsBusiness(this ClaimsPrincipal claims)
        {
            if (claims != null)
            {
                var result = claims?.FindFirst("BusinessEnable")?.Value;
                if(!string.IsNullOrEmpty(result))
                {
                   if(result == "1")
                        return true;
                   else if(result == "0")
                        return false;   

                }
            }
            return default;
        }

        public static T? GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            var userId = identity?.GetUserId();

            return userId.HasValue()
                ? (T)Convert.ChangeType(userId, typeof(T), CultureInfo.InvariantCulture)
                : default;
        }

        public static string? GetUserName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Name);
        }

        public static string GetToken(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst("Token");
                return result.Value;
            }
            return default;
        }

        public static int GetPersonId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst("PersonId");
                return result.Value.ToInt();
            }
            return default;
        }

        public static string GetNationalCode(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                var result = claimsPrincipal.FindFirst("NationalCode");
                return result.Value;
            }
            return default;
        }

        public static string? GetRoleName(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.Role);
        }

        public static string? GetMobile(this IIdentity identity)
        {
            return identity?.FindFirstValue(ClaimTypes.MobilePhone);
        }

    }
}
