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
public class DoctorController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwt;

    public DoctorController(AppDbContext context, JwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    [HttpPost("change-availability")]
    public async Task<IActionResult> ChangeAvailability([FromForm] DoctorDto dto)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.Id);
        
    }
}
