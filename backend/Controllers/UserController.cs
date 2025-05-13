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

    [HttpGet("get-profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var token = Request.Headers["token"].FirstOrDefault();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { success = false, message = "Token is missing" });
        }

        var principal = _jwt.ValidateToken(token);
        if (principal == null)
        {
            return Unauthorized(new { success = false, message = "Invalid token" });
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { success = false, message = "User ID claim not found" });
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized(new { success = false, message = "Invalid user ID in token" });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { success = false, message = "User not found" });
        }

        return Ok(new { success = true, user });
    }

}
