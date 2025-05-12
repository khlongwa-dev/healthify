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

    
}
