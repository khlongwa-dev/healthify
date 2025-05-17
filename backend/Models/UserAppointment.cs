using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class UserAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public required string SlotDate { get; set; }
        public required string SlotTime { get; set; }
        
        public  Doctor? Doctor { get; set; }
        public  User? User { get; set; }

        public int DoctorFee { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public bool Cancelled { get; set; } = false;
        public bool Paid { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
    }
}