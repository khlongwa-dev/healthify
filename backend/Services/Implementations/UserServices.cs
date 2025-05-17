using System.IdentityModel.Tokens.Jwt;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserFromTokenAsync(string? token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");
                if (userIdClaim == null)
                    return null;

                int userId = int.Parse(userIdClaim.Value);
                return await _context.Users.FindAsync(userId);
            }
            catch
            {
                return null;
            }
        }


    }
}