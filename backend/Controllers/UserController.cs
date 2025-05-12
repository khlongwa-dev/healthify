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
    public async Task<IActionResult> CreateUser([FromForm] UserRegisterDto dto)
    {
        if (_context.Users.Any(d => d.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email already exists." });

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = passwordHash,
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            success = true,
            message = "User created successfully.",
            user,
            token
        });
    }
}
