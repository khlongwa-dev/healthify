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

    [HttpPost("profile")]
    public async Task<IActionResult> GetUserProfile([FromBody] Dictionary<string, int> body)
    {
        if (!body.TryGetValue("userId", out int userId))
        {
            return BadRequest(new { success = false, message = "Missing user ID." });
        }

        var user = await _context.Users.FirstOrDefaultAsync(d => d.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found." });
        }

        await _context.SaveChangesAsync();

        return Ok(new { success = true, user });
    }

    [HttpPost("update-profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDto dto)
    {
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
        return Ok(new { success = true, message = "Doctor availability updated.", user });
    }
}
