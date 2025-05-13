namespace backend.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
        
        public required Doctor Doctor { get; set; }
        public required User User { get; set; }

        public int Appointments { get; set; } // number of appointments
        public required DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public bool Cancelled { get; set; } = false;
        public bool Paid { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
    }
}