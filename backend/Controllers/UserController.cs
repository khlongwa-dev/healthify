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

        [HttpPost("clear-appointment")]
        public async Task<IActionResult> ClearAppointment([FromBody] Dictionary<string, int> body)
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);
            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });

            var appointmentId = body.GetValueOrDefault("appointmentId");

            bool clear = await _deps.UserService.ClearUserAppointmentByIdAsync(appointmentId, user.Id);

            return clear
                    ? Ok(new { success = true, message = "Appointment cleared successfully." })
                    : Ok(new { success = false, message = "Appointment not found." });
        }
        
        [HttpGet("get-appointments")]
        public async Task<IActionResult> GetUserAppointments()
        {
            string? token = Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var user = await _deps.UserService.GetUserFromTokenAsync(token);

            if (user == null)
                return Ok(new { success = false, message = "Not authorized." });


            var appointments = (await _deps.UserService
                .GetUserAppointmentsByIdAsync(user.Id))
                .OrderByDescending(a => a.Id)
                .ToList();

            return Ok(new { success = true, appointments});
        }
    }
}