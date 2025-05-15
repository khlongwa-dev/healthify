
namespace backend.DTOs
{
    public class UpdateDoctorProfileDto
    {
        public bool Available { get; set; }
        public required int Fees { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
    }
}