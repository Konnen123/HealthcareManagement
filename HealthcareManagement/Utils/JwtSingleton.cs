using Domain.Utils;
using System.Security.Claims;

namespace HealthcareManagement.Utils
{
    public class JwtSingleton
    {
        private JwtSingleton()
        {

        }

        public static Result<Guid> GetUserIdFromJwt(ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if(claim == null)
            {
                return Result<Guid>.Failure(new Error("Jwt-token", "Claim not found"));
            }

            Guid userId = new Guid(claim.Value);

            return Result<Guid>.Success(userId);
        }
    }
}
