using System.Security.Claims;
using System.Threading.Tasks;

using App1.MobileAppService.Models;
using App1.MobileAppService.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace App1.MobileAppService.Controllers
{

    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public TokenController(
            IJwtTokenService tokenService,
            UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TokenResult>> Auth([FromForm] string email, [FromForm] string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(email);
            bool validCredentials = await _userManager.CheckPasswordAsync(user, password);

            if (!validCredentials)
            {
                return BadRequest("Username or password is incorrect");
            }

            string newJwtToken = GenerateToken(email);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenResult
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<ActionResult<TokenResult>> Refresh([FromForm] string token, [FromForm] string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            string email = GetEmailFromClaimsPrincipal(principal);

            var user = await _userManager.FindByEmailAsync(email);

            string savedRefreshToken = user.RefreshToken;

            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            string newJwtToken = GenerateToken(email);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenResult
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        private static string GetEmailFromClaimsPrincipal(ClaimsPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.Email).Value;
        }

        private string GenerateToken(string email)
        {
            return _tokenService.BuildToken(email);
        }

        private string GenerateRefreshToken()
        {
            return _tokenService.GenerateRefreshToken();
        }
    }
}
