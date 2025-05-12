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
}
