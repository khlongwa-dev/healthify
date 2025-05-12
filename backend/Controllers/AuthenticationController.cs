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
public class AuthenticationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;

    public AuthenticationController(AppDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    [HttpPost("doctor/login")]
    public IActionResult DoctorLogin([FromBody] LoginDto dto)
    {
        var doctor = _context.Doctors.FirstOrDefault(d => d.Email == dto.Email);
        
        if (doctor == null || !BCrypt.Net.BCrypt.Verify(dto.Password, doctor.Password))
            
            return Unauthorized(new { success = false, message = "Invalid email or password." });

        var token = _jwt.GenerateToken(doctor);

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            token = token
        });
    }

    [HttpPost("user/login")]
    public IActionResult UserLogin([FromBody] LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(d => d.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            
            return Unauthorized(new { success = false, message = "Invalid email or password." });

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            token = token
        });
    }

    [HttpPost("admin/login")]
    public IActionResult AdminLogin([FromBody] LoginDto dto)
    {
        var admin = _context.Admins.FirstOrDefault(d => d.Email == dto.Email);
        
        if (admin == null || !BCrypt.Net.BCrypt.Verify(dto.Password, admin.Password))
            
            return Unauthorized(new { success = false, message = "Invalid email or password." });

        var token = _jwt.GenerateToken(admin);

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            token = token
        });
    }

    [HttpPost("user/register")]
    public async Task<IActionResult> CreateUser([FromBody] UserRegisterDto dto)
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
