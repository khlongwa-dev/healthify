using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using backend.DTOs;
using backend.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly Cloudinary _cloudinary;
    private readonly JwtService _jwt;

    public UserController(AppDbContext context, Cloudinary cloudinary, JwtService jwt)
    {
        _context = context;
        _cloudinary = cloudinary;
        _jwt = jwt;
    }

    [HttpGet("get-profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var token = Request.Headers["token"].FirstOrDefault();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { success = false, message = "Token is missing" });
        }

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
        {
            return Unauthorized(new { success = false, message = "Invalid token" });
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { success = false, message = "User ID claim not found" });
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        return Ok(new { success = true, user });
    }

    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto dto)
    {
        var token = Request.Headers["token"].FirstOrDefault();
       
        if (string.IsNullOrEmpty(token)) return Unauthorized(new { success = false, message = "Invalid token" });

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
        {
            return Unauthorized(new { success = false, message = "Invalid token" });
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { success = false, message = "User ID claim not found" });
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        string imageUrl = "";
        if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.ImageUrl.FileName, dto.ImageUrl.OpenReadStream()),
                Folder = "doctor_profiles"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            imageUrl = uploadResult.SecureUrl.ToString();
        }

        user.Name = dto.Name;
        user.Phone = dto.Phone;
        user.AddressLine1 = dto.AddressLine1;
        user.AddressLine2 = dto.AddressLine2;
        user.Gender = dto.Gender;
        user.DoB = dto.DoB;

        if (!string.IsNullOrEmpty(imageUrl)) {
            user.ImageUrl = imageUrl;
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true, message = "Profile updated", user });
    }

    [HttpPost("book-appointment")]
    public async Task<IActionResult> BookAppointment([FromBody] BookingAppointmentDto dto)
    {
        var token = Request.Headers["token"].FirstOrDefault();
       
        if (string.IsNullOrEmpty(token)) return Unauthorized(new { success = false, message = "Invalid token" });

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
        {
            return Unauthorized(new { success = false, message = "Invalid token" });
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { success = false, message = "User ID claim not found" });
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        if (!int.TryParse(dto.DoctorId, out int docId))
            return BadRequest(new { success = false, message = "Invalid doctor ID format" });

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == docId);

        if (doctor == null)
            return NotFound(new { success = false, message = "Doctor not found" });
        
        if (!doctor.Available)
            return BadRequest(new { success = false, message = "Doctor is not available." });


        var bookedSlots = string.IsNullOrWhiteSpace(doctor.SlotsBooked)
            ? new Dictionary<string, List<string>>()
            : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(doctor.SlotsBooked)
                ?? new Dictionary<string, List<string>>();

        
        if (bookedSlots.TryGetValue(dto.SlotDate, out var times))
        {
            if (times.Contains(dto.SlotTime))
                return BadRequest(new { success = false, message = "Slot not available" });

            times.Add(dto.SlotTime);
        }
        else
        {
            bookedSlots[dto.SlotDate] = new List<string> { dto.SlotTime };
        }

        

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

    }
}
