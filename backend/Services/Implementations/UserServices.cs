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


        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileDto dto, string? imageUrl)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Name = dto.Name;
            user.Phone = dto.Phone;
            user.AddressLine1 = dto.AddressLine1;
            user.AddressLine2 = dto.AddressLine2;
            user.Gender = dto.Gender;
            user.DoB = dto.DoB;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                user.ImageUrl = imageUrl;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserAppointment>> GetUserAppointmentsByIdAsync(int userId)
        {
            return await _context.UserAppointments
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.Id)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        
    }
}