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
        public async Task<IActionResult> GetAdminDashBoardData()
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

        [HttpGet("all-appointments")]
        public async Task<IActionResult> GetAllAppointments()
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
                return Unauthorized(new { success = false, message = "Invalid admin ID in token" });
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Id == adminId);
            if (admin == null)
            {
                return NotFound(new { success = false, message = "Admin not found" });
            }

            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                appointments
            });
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