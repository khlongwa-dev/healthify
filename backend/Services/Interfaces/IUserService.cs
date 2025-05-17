using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserFromTokenAsync(string? token);
        Task<bool> UpdateProfileAsync(int userId, UpdateUserProfileDto dto, string? imageUrl);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> ClearUserAppointmentByIdAsync(int appointmentId, int userId);
        Task<List<UserAppointment>> GetUserAppointmentsByIdAsync(int callerId);
    }
}