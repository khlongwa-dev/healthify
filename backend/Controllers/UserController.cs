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

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromForm] UserDto dto)
    {
        if (_context.Users.Any(d => d.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email already exists." });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        string imageUrl = "";
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(dto.Image.FileName, dto.Image.OpenReadStream()),
                Folder = "user_profiles"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            imageUrl = uploadResult.SecureUrl.ToString();
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = passwordHash,
            ImageUrl = imageUrl,
            DoB = dto.DoB,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            Gender = dto.Gender,
            Phone = dto.Phone
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "User created successfully.",
            user
        });
    }
}
