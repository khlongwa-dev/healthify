using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Phone { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string Gender { get; set; }

        [Required]
        public required string DoB { get; set; }

        // Optional image (form field name must match)
        public IFormFile? ImageUrl { get; set; }
    }
}
