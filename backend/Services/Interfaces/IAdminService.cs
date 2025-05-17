using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> CreateDoctorAsync(CreateDoctorDto dto, string? imageUrl);
        Task<Admin?> GetAdminFromTokenAsync(string? token);
    }
}