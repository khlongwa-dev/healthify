using System.IdentityModel.Tokens.Jwt;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetDoctorFromTokenAsync(string? token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");
                if (userIdClaim == null)
                    return null;

                int doctorId = int.Parse(userIdClaim.Value);
                return await _context.Doctors.FindAsync(doctorId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task<bool> UpdateProfileAsync(int doctorId, UpdateDoctorProfileDto dto)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null) return false;

            doctor.AddressLine1 = dto.AddressLine1;
            doctor.AddressLine2 = dto.AddressLine2;
            doctor.Available = dto.Available;
            doctor.Fees = dto.Fees;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Doctors
                .Include(d => d.BookedSlots)
                .ToListAsync();

            return doctors;
        }

        
    }
}