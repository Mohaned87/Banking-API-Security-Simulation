using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MiniBank.Api.Models;
using MiniBank.Api.Dtos;
using MiniBank.Api.Data;
using static MiniBank.Api.Dtos.AuthDtos;

namespace MiniBank.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDb _db;
        private readonly IConfiguration _cfg;

        public AuthService(AppDb db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already used");

            var customer = new Customer { FullName = dto.FullName, NationalId = "" };
            var user = new AppUser
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Customer = customer
            };
            _db.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("customerId", user.CustomerId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpiresMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

