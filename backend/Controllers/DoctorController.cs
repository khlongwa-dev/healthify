using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using backend.DTOs;
using backend.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using backend.Dependencies;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly HealthifyDependencies _deps;

        public DoctorController(HealthifyDependencies deps)
        {
            _deps = deps;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var doctor = await _deps.DoctorService.GetDoctorFromTokenAsync(token);

            if (doctor == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointments = (await _deps.DoctorService
                .GetDoctorAppointmentsByIdAsync(doctor.Id))
                .OrderByDescending(a => a.Id)
                .ToList();

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

            var appointmentCount = appointments
                .Count(a => !a.IsCompleted && !a.Cancelled);

            return Ok(new
            {
                success = true,
                dashdata = new
                {
                    earnings,
                    patientCount,
                    appointmentCount,
                    latestAppointments
                }
            });
        }

        [HttpGet("get-profile")]
        public async Task<IActionResult> GetDoctorProfile()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var doctor = await _deps.DoctorService.GetDoctorFromTokenAsync(token);
            if (doctor == null)
                return Ok(new { success = false, message = "Not authorized." });

            return Ok(new { success = true, doctor });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateDoctorProfileDto dto)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var doctor = await _deps.DoctorService.GetDoctorFromTokenAsync(token);
            if (doctor == null)
                return Ok(new { success = false, message = "Not authorized." });

            var updated = await _deps.DoctorService.UpdateProfileAsync(doctor.Id, dto);
            if (!updated)
                Ok(new { success = false, message = "Failed to update profile." });

            return Ok(new { success = true, message = "Profile updated successfully." }); ;
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
                doctor.Available,
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

        [HttpGet("get-appointments")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var doctor = await _deps.DoctorService.GetDoctorFromTokenAsync(token);

            if (doctor == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointments = await _deps.DoctorService.GetDoctorAppointmentsByIdAsync(doctor.Id);

            var formatted = appointments.Select(a => new
            {
                a.Id,
                a.SlotDate,
                a.SlotTime,
                DoctorFee = a.Doctor?.Fees,
                a.Cancelled,
                a.Paid,
                a.IsCompleted,
                a.Date,
                a.Doctor,
                User = a.User == null ? null : new
                {
                    a.User.Id,
                    a.User.Name,
                    a.User.AddressLine1,
                    a.User.AddressLine2,
                    a.User.DoB,
                    a.User.ImageUrl
                }
            });

            return Ok(new { success = true, appointments });
        }

        [HttpPut("cancel-appointment")]
        public async Task<IActionResult> CancelAppointment([FromBody] Dictionary<string, int> body)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var doctor = await _deps.DoctorService.GetDoctorFromTokenAsync(token);
            if (doctor == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointmentId = body.GetValueOrDefault("appointmentId");

            bool cancel = await _deps.AppointmentService.CancelAppointmentAsync(appointmentId, doctor.Id, "Doctor");

            return cancel
                    ? Ok(new { success = true, message = "Appointment cancelled successfully." })
                    : Ok(new { success = false, message = "Appointment not found." });
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

        
    }
}