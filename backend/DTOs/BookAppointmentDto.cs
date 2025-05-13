using backend.Models;

namespace backend.DTOs
{
    public class BookingAppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
        
        public required Doctor Doctor { get; set; }
        public required User User { get; set; }
        public int DoctorFee { get; set; }
    
    }
}