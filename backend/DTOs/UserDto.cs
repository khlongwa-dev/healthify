using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UserDto
    {
        public required string Name { get; set; }
        
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ImageUrl { get; set; } 
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public string Gender = "Not Selected";
        public required DateOnly DoB { get; set; }
        public required string Phone { get; set; }
    }
}