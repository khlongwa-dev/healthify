using backend.Models;

namespace backend.DTOs
{
    public class BookingAppointmentDto
    {
        public int DoctorId { get; set; }
        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
    
    }
}