using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task<Doctor?> GetDoctorFromTokenAsync(string? token);
        Task<List<Doctor>> GetAllDoctorsAsync();
        Task<bool> UpdateProfileAsync(int doctorId, UpdateDoctorProfileDto dto);
        Task<bool> ChangeDoctorAvailabilityAsync(int doctorId);
        Task<List<Appointment>> GetDoctorAppointmentsByIdAsync(int doctorId);
    }
}