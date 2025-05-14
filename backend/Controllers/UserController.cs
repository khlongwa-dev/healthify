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
        if (string.IsNullOrEmpty(token)) 
            return Unauthorized(new { success = false, message = "Invalid token" });

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
            return Unauthorized(new { success = false, message = "Invalid token" });

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return NotFound(new { success = false, message = "User not found" });

        if (!int.TryParse(dto.DoctorId, out int docId))
            return BadRequest(new { success = false, message = "Invalid doctor ID format" });

        var doctor = await _context.Doctors
            .Include(d => d.BookedSlots)
            .FirstOrDefaultAsync(d => d.Id == docId);

        if (doctor == null)
            return NotFound(new { success = false, message = "Doctor not found" });

        if (!doctor.Available)
            return BadRequest(new { success = false, message = "Doctor is not available." });

        // Check if slot already booked
        bool slotExists = doctor.BookedSlots
            .Any(bs => bs.SlotDate == dto.SlotDate && bs.SlotTime == dto.SlotTime);

        if (slotExists)
            return BadRequest(new { success = false, message = "Slot not available" });

        // Add new booked slot
        var bookedSlot = new BookedSlot
        {
            DoctorId = doctor.Id,
            SlotDate = dto.SlotDate,
            SlotTime = dto.SlotTime
        };
        await _context.BookedSlots.AddAsync(bookedSlot);

        // Create appointment
        var appointment = new Appointment
        {
            UserId = userId,
            DoctorId = doctor.Id,
            SlotDate = dto.SlotDate,
            SlotTime = dto.SlotTime,
            Doctor = doctor,
            User = user,
            DoctorFee = doctor.Fees,
            Date = DateOnly.FromDateTime(DateTime.Today),
            Cancelled = false,
            Paid = false,
            IsCompleted = false
        };

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Appointment booked successfully" });
    }

    [HttpGet("get-appointments")]
    public async Task<IActionResult> GetUserAppointments()
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
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        var appointments = await _context.Appointments
            .Where(a => a.UserId == userId)
            .Include(a => a.Doctor)
            .Include(a => a.User)
            .Select(a => new
            {
                a.Id,
                a.SlotDate,
                a.SlotTime,
                a.DoctorFee,
                a.Cancelled,
                a.Paid,
                a.IsCompleted,
                a.Date,
                Doctor = new
                {
                    a.Doctor.Id,
                    a.Doctor.Name,
                    a.Doctor.AddressLine1,
                    a.Doctor.AddressLine2,
                    a.Doctor.Specialty,
                    a.Doctor.ImageUrl
                },
                User = new
                {
                    a.User.Id,
                    a.User.Name,
                    a.User.Email
                }
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            appointments
        });
    }

    [HttpPost("cancel-appointment")]
    public async Task<IActionResult> CancellAppointment([FromBody] Dictionary<string, int> body)
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
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        if (!body.TryGetValue("appointmentId", out int appointmentId))
        {
            return BadRequest(new { success = false, message = "Missing appointment ID." });
        }

        // find the appointment
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && a.UserId == userId);
        
        if (appointment == null)
        {
            return NotFound(new { success = false, message = "Appointment not found or does not belong to the user." });
        }

        //cancel appointment
        appointment.Cancelled = true;

        // find a booked slot
        var bookedSlot = await _context.BookedSlots.FirstOrDefaultAsync(bs =>
            bs.DoctorId == appointment.DoctorId &&
            bs.SlotDate == appointment.SlotDate &&
            bs.SlotTime == appointment.SlotTime);
        
        // remove booked slot
        if (bookedSlot != null)
        {
            _context.BookedSlots.Remove(bookedSlot);
        }
    }
}
