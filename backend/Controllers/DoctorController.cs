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
    public async Task<IActionResult> ChangeAvailability([FromBody] DoctorDto dto)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == dto.Id);
        if (doctor == null)
    {
        return NotFound(new { message = "Doctor not found." });
    }

    doctor.Availabe = !doctor.Availabe;

    await _context.SaveChangesAsync();

    return Ok(new { success = true, message = "Doctor availability updated.", availability = doctor.Availabe });
    }
}
