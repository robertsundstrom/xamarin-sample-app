using System.Security.Claims;

namespace App1.MobileAppService.Services
{
    public interface IJwtTokenService
    {
        string BuildToken(string email);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}