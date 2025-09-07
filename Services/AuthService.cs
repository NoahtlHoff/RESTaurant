using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESTaurang.Data;
using RESTaurang.Models;
using RESTaurang.Services.IServices;

namespace RESTaurang.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _ctx;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var admin = await _ctx.Admins.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Username == username);

            if (admin is null) return null;
            if (!VerifyPassword(password, admin.PasswordHash, admin.PasswordSalt)) return null;

            return GenerateJwt(admin.Username);
        }

        private bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(computed);
        }

        private string GenerateJwt(string username)
        {
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
