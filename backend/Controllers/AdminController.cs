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
    public class AdminController : ControllerBase
    {
        private readonly HealthifyDependencies _deps;

        public AdminController(HealthifyDependencies deps)
        {
            _deps = deps;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAdminDashboardData()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var admin = await _deps.AdminService.GetAdminFromTokenAsync(token);

            if (admin == null)
                return Ok(new { success = false, message = "Not authorized." });

            var doctors = await _deps.DoctorService.GetAllDoctorsAsync();
            var doctorCount = doctors.Count;

            var appointments = await _deps.AppointmentService.GetAllAppointmentsAsync();
            var appointmentCount = appointments.Count;

            var users = await _deps.UserService.GetAllUsersAsync();
            var userCount = users.Count;

            var latestAppointments = appointments
                .OrderByDescending(a => a.Id)
                .Take(5);

            return Ok(new
            {
                success = true,
                dashdata = new
                {
                    doctorCount,
                    appointmentCount,
                    userCount,
                    latestAppointments,
                }
            });
        }

        [HttpGet("doctors-list")]
        public async Task<IActionResult> GetDoctorList()
        {
            var doctors = await _deps.DoctorService.GetAllDoctorsAsync();

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

        [HttpPost("add-doctor")]
        public async Task<IActionResult> AddDoctor([FromForm] CreateDoctorDto dto)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var admin = await _deps.AdminService.GetAdminFromTokenAsync(token);

            if (admin == null)
                return Ok(new { success = false, message = "Not authorized." });

            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                imageUrl = await Helpers.CloudinaryHelper.UploadImageAsync(_deps.Cloudinary, dto.Image, "doctor_profiles");
            }

            bool addDoctor = await _deps.AdminService.CreateDoctorAsync(dto, imageUrl);

            return addDoctor
                        ? Ok(new { success = true, message = "Doctor added successfully." })
                        : Ok(new { success = false, message = "Email already exist." });
        }

        [HttpPut("change-availability")]
        public async Task<IActionResult> ChangeDoctorAvailability([FromBody] Dictionary<string, int> body)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var admin = await _deps.AdminService.GetAdminFromTokenAsync(token);
            if (admin == null)
                return Ok(new { success = false, message = "Not authorized." });

            var doctorId = body.GetValueOrDefault("doctorId");

            bool changeDoctorAvailability = await _deps.DoctorService.ChangeDoctorAvailabilityAsync(doctorId);

            return changeDoctorAvailability
                    ? Ok(new { success = true, message = "Doctor availability changed." })
                    : Ok(new { success = false, message = "Doctor not found." });
        }

        [HttpGet("appointments-list")]
        public async Task<IActionResult> GetAllAppointmentList()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var admin = await _deps.AdminService.GetAdminFromTokenAsync(token);

            if (admin == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointments = (await _deps.AppointmentService
                .GetAllAppointmentsAsync())
                .OrderByDescending(a => a.Id)
                .ToList();

            return Ok(new { success = true, appointments });
        }

        [HttpPost("cancel-appointment")]
        public async Task<IActionResult> CancelAppointment([FromBody] Dictionary<string, int> body)
        {
            var token = Request.Headers["aToken"].FirstOrDefault();
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
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int adminId))
            {
                return Unauthorized(new { success = false, message = "Invalid user ID in token" });
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
            if (admin == null)
            {
                return NotFound(new { success = false, message = "Admin not found" });
            }

            if (!body.TryGetValue("appointmentId", out int appointmentId))
            {
                return BadRequest(new { success = false, message = "Missing appointment ID." });
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


    }
}