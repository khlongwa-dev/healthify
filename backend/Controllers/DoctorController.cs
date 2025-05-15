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
public class DoctorController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;

    public DoctorController(AppDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetDoctorList()
    {
        var doctors = await _context.Doctors
        .Include(d => d.BookedSlots)
        .ToListAsync();

        var result = doctors.Select(doctor => new
        {
            doctor.Id,
            doctor.Name,
            doctor.Email,
            doctor.Specialty,
            doctor.Degree,
            doctor.Experience,
            doctor.Fees,
            doctor.About,
            doctor.ImageUrl,
            doctor.AddressLine1,
            doctor.AddressLine2,
            BookedSlots = doctor.BookedSlots
                .GroupBy(bs => bs.SlotDate)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(bs => bs.SlotTime).ToList()
                )
        });

        return Ok(new
        {
            success = true,
            doctors = result
        });
    }

    [HttpGet("appointments")]
    public async Task<IActionResult> GetUserAppointments()
    {
        var token = Request.Headers["dToken"].FirstOrDefault();
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
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int docId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == docId);
        if (doctor == null)
        {
            return NotFound(new { success = false, message = "Doctor not found doc " });
        }

        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == docId)
            .Include(a => a.User)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

        return Ok(new
        {
            success = true,
            appointments
        });
    }


    [HttpPost("complete-appointment")]
    public async Task<IActionResult> CompleteAppointment([FromBody] Dictionary<string, int> body)
    {
        var token = Request.Headers["dToken"].FirstOrDefault();
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
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int docId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var admin = await _context.Doctors.FirstOrDefaultAsync(a => a.Id == docId);
        if (admin == null)
        {
            return NotFound(new { success = false, message = "Admin not found" });
        }

        if (!body.TryGetValue("appointmentId", out int appointmentId))
        {
            return BadRequest(new { success = false, message = "Missing doctor ID." });
        }

        var appointment = await _context.Appointments.FirstOrDefaultAsync(d => d.Id == appointmentId);
        if (appointment == null)
        {
            return NotFound(new { success = false, message = "Appointment not found." });
        }

        appointment.IsCompleted = true;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Appointment completed." });
    }

    [HttpPost("cancel-appointment")]
    public async Task<IActionResult> CancelAppointment([FromBody] Dictionary<string, int> body)
    {
        var token = Request.Headers["dToken"].FirstOrDefault();
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
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int docId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var admin = await _context.Doctors.FirstOrDefaultAsync(a => a.Id == docId);
        if (admin == null)
        {
            return NotFound(new { success = false, message = "Admin not found" });
        }

        if (!body.TryGetValue("appointmentId", out int appointmentId))
        {
            return BadRequest(new { success = false, message = "Missing doctor ID." });
        }

        // find the appointment
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

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

        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Appointment cancelled successfully." });
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardData()
    {
        var token = Request.Headers["dToken"].FirstOrDefault();
        if (string.IsNullOrEmpty(token))
            return Unauthorized(new { success = false, message = "Token is missing" });

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
            return Unauthorized(new { success = false, message = "Invalid token" });

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int docId))
            return Unauthorized(new { success = false, message = "Invalid doctor ID in token" });

        // Validate doctor exists
        bool doctorExists = await _context.Doctors.AnyAsync(d => d.Id == docId);
        if (!doctorExists)
            return NotFound(new { success = false, message = "Doctor not found" });

        // Query appointments directly
        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == docId)
            .Include(a => a.User)
            .Include(a => a.Doctor)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

        var earnings = appointments
            .Where(a => a.Paid || a.IsCompleted)
            .Sum(a => a.DoctorFee);

        var patientCount = appointments
            .Select(a => a.User?.Id)
            .Where(id => id.HasValue)
            .Distinct()
            .Count();

        var latestAppointments = appointments
            .Take(5);

        return Ok(new
        {
            success = true,
            dashdata = new
            {
                earnings,
                patients = patientCount,
                appointmentCount = appointments.Count,
                latestAppointments
            }
        });
    }
    
    [HttpGet("get-profile")]
    public async Task<IActionResult> GetDoctorProfile()
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
            return Unauthorized(new { success = false, message = "Doctor ID claim not found" });
        }

        if (!int.TryParse(userIdClaim.Value, out int docId))
        {
            return Unauthorized(new { success = false, message = "Invalid doctor ID in token" });
        }

        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == docId);
        if (doctor == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        return Ok(new { success = true, doctor });
    }

}
