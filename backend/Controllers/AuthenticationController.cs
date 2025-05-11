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

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var doctor = _context.Doctors.FirstOrDefault(d => d.Email == dto.Email);
        
        if (doctor == null || !BCrypt.Net.BCrypt.Verify(dto.Password, doctor.Password))
            
            return Unauthorized("Invalid email or password.");

        var token = _jwt.GenerateToken(doctor);

        return Ok(new { token });
    }
}
