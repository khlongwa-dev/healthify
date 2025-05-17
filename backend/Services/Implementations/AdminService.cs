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

        public async Task<bool> CreateDoctorAsync(CreateDoctorDto dto, string? imageUrl)
        {
            if (_context.Doctors.Any(d => d.Email == dto.Email))
                return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var doctor = new Doctor
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = passwordHash,
                ImageUrl = imageUrl ?? "",
                Specialty = dto.Specialty,
                Degree = dto.Degree,
                Experience = dto.Experience,
                About = dto.About,
                Fees = dto.Fees,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}