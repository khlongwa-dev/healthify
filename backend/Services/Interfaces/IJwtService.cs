using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IJwtService
    {
        int? ValidateToken(string? token);
        string GenerateToken(User user);
        string GenerateToken(Doctor doctor);
        string GenerateToken(Admin admin);
    }
}