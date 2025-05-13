namespace backend.Models
{   
    public class BookedSlot
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
    }
}