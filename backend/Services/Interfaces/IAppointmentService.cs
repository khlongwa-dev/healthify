using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<bool> BookAppointmentAsync(BookingAppointmentDto dto, int userId);
        Task<bool> CancelAppointmentAsync(int appointmentId, int? callerId, string callerRole);
        Task<bool> CompleteAppointmentAsync(int appointmentId, int callerId);
        Task<bool> ClearAppointmentAsync(int appointmentId);
        Task<List<Appointment>> GetAllAppointmentsAsync();
    }
}