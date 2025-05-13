using backend.Models;

namespace backend.DTOs
{
    public class BookingAppointmentDto
    {
        public required string DoctorId { get; set; }
        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
    
    }
}