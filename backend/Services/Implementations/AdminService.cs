using System.IdentityModel.Tokens.Jwt;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;

namespace backend.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetAdminFromTokenAsync(string? token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");
                if (userIdClaim == null)
                    return null;

                int adminId = int.Parse(userIdClaim.Value);
                return await _context.Admins.FindAsync(adminId);
            }
            catch
            {
                return null;
            }
        }

    }
}