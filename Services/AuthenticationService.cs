using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.DTO;
using ArtMarketPlaceAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArtMarketPlaceAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(Register registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("email déjà utilisé");

            CreatePasswordHash(registerDto.Mdp, out byte[] hash, out byte[] salt);

            var user = new User
            {
                NomComplet = registerDto.NomComplet,
                Email = registerDto.Email,
                MdpHash = hash,
                MdpSalt = salt,
                Role = registerDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Utilisateur enregistré";
        }

        public async Task<object> LoginAsync(Login loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                throw new Exception("Utilisateur non trouvé");

            if (!VerifyPasswordHash(loginDto.Mdp, user.MdpHash, user.MdpSalt))
                throw new Exception("Mot de passe incorrect");

            var token = GenerateJwtToken(user);

            return new
            {
                token,
                email = user.Email,
                role = user.Role,
                userId = user.Id // 👈 pratique pour debug côté Angular
            };
        }

        private void CreatePasswordHash(string mdp, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdp));
        }

        private bool VerifyPasswordHash(string mdp, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(mdp));
            return computedHash.SequenceEqual(hash);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // 👈 ID inclus dans le token
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
