using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ImageUrl { get; set; }
        public required string Specialty { get; set; }
        public required string Degree { get; set; }
        public required string Experience { get; set; }
        public required string About { get; set; }
        public required bool Availabe { get; set; }
        public required int Fees { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public  DateOnly Date { get; set; }
    }
}