using System.Security.Claims;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new InvalidOperationException("Claim not found");
            return claim.Value;
        }
    }
}
