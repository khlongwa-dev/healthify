using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using backend.DTOs;
using backend.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly Cloudinary _cloudinary;
    private readonly JwtService _jwt;

    public AdminController(AppDbContext context, Cloudinary cloudinary, JwtService jwt)
    {
        _context = context;
        _cloudinary = cloudinary;
        _jwt = jwt;
    }

    [HttpPost("add-doctor")]
    public async Task<IActionResult> CreateDoctor([FromForm] DoctorDto dto)
    {
        if (_context.Doctors.Any(d => d.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email already exists." });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        string imageUrl = "";
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.Image.FileName, dto.Image.OpenReadStream()),
                Folder = "doctor_profiles"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            imageUrl = uploadResult.SecureUrl.ToString();
        }

        var doctor = new Doctor
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = passwordHash,
            ImageUrl = imageUrl,
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

        return Ok(new
        {
            success = true,
            message = "Doctor created successfully.",
            doctor
        });
    }

    [HttpPost("all-doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _context.Doctors.ToListAsync();
        return Ok(new
        {
            success = true,
            doctors
        });
    }

    [HttpPost("change-availability")]
    public async Task<IActionResult> ChangeAvailability([FromBody] Dictionary<string, int> body)
    {
        if (!body.TryGetValue("docId", out int docId))
        {
            return BadRequest(new { success = false, message = "Missing doctor ID." });
        }

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == docId);
        if (doctor == null)
        {
            return NotFound(new { success = false, message = "Doctor not found." });
        }

        doctor.Available = !doctor.Available;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Doctor availability updated.", availability = doctor.Available });
    }
}
