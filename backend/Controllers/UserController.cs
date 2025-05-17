using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using backend.DTOs;
using backend.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using backend.Dependencies;
using backend.Helpers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly HealthifyDependencies _deps;

        public UserController(HealthifyDependencies deps)
        {
            _deps = deps;
        }

        [HttpGet("get-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);
            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });

            return Ok(new { success = true, user });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateUserProfileDto dto)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);
            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });

            string? imageUrl = null;
            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                imageUrl = await CloudinaryHelper.UploadImageAsync(_deps.Cloudinary, dto.ImageUrl, "user_profiles");
            }

            var updated = await _deps.UserService.UpdateProfileAsync(user.Id, dto, imageUrl);
            if (!updated)
                Ok(new { success = false, message = "Failed to update profile." });

            return Ok(new { success = true, message = "Profile updated successfully." }); ;
        }

        [HttpPost("book-appointment")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingAppointmentDto dto)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);
            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });

            bool book = await _deps.AppointmentService.BookAppointmentAsync(dto, user.Id);

            return book
                    ? Ok(new { success = true, message = "Appointment booked successfully." }) 
                    : Ok(new { success = false, message = "Doctor not available." });
        }

        [HttpPut("cancel-appointment")]
        public async Task<IActionResult> CancelAppointment([FromBody] Dictionary<string, int> body)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);
            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointmentId = body.GetValueOrDefault("appointmentId");    
        
            bool cancel = await _deps.AppointmentService.CancelAppointmentAsync(appointmentId, user.Id, "User");

            return cancel
                    ? Ok(new { success = true, message = "Appointment cancelled successfully." }) 
                    : Ok(new { success = false, message = "Appointment not found." });
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

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Appointment cancelled successfully." });
        }
    }
}