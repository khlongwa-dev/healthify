using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateDoctorDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
         
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required IFormFile Image { get; set; }
        public required string Specialty { get; set; }
        public required string Degree { get; set; }
        public required string Experience { get; set; }
        public required string About { get; set; }
        public int Fees { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required DateOnly Date { get; set; }
    }
}