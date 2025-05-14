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

    [HttpGet("list")]
    public async Task<IActionResult> GetDoctorList()
    {
        var doctors = await _context.Doctors
        .Include(d => d.BookedSlots)
        .ToListAsync();

        var result = doctors.Select(doctor => new
        {
            doctor.Id,
            doctor.Name,
            doctor.Email,
            doctor.Specialty,
            doctor.Degree,
            doctor.Experience,
            doctor.Fees,
            doctor.About,
            doctor.ImageUrl,
            doctor.AddressLine1,
            doctor.AddressLine2,
            BookedSlots = doctor.BookedSlots
                .GroupBy(bs => bs.SlotDate)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(bs => bs.SlotTime).ToList()
                )
        });

        return Ok(new
        {
            success = true,
            doctors = result
        });
    }
}
