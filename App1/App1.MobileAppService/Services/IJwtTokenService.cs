using System.Security.Claims;

using App1.MobileAppService.Models;

namespace App1.MobileAppService.Services
{
    public interface IJwtTokenService
    {
        string BuildToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
