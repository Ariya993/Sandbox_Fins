using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity.Data;

namespace Sandbox_Calc.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly Services _services;
        private readonly AppDbContext _appDbContext;
        public AuthController(Services service,AppDbContext appDbContext)
        {
            _services = service;
            _appDbContext= appDbContext;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            string hashedPassword = HashHelpers.ToMd5(request.Password);

            var user = await _appDbContext.APPUSER
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == hashedPassword);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var (token, expiresInMinutes) = _services.GenerateToken(user.Username!);
            var refreshToken = _services.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                token,
                expiresIn = expiresInMinutes,
                refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var user = await _appDbContext.APPUSER
                .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token.");

            var (newToken, expiresInMinutes) = _services.GenerateToken(user.Username!);
            var newRefreshToken = _services.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                token = newToken,
                expiresIn = expiresInMinutes,
                refreshToken = newRefreshToken
            });
        }


        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok("This is a protected route");
        }
    }
}
