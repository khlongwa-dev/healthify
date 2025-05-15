using System.ComponentModel.DataAnnotations;
using backend.Models.Interfaces;

namespace backend.DTOs
{
    public class UpdateDoctorProfileDto
    {
        public bool Available { get; set; } = true;
        public required int Fees { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
    }
}