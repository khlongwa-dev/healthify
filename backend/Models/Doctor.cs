using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Interfaces;
using System.Text.Json;

namespace backend.Models
{
    public class Doctor : IUser
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
        public bool Available { get; set; } = true;
        public required int Fees { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public  DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string AvailableSlotsJson { get; set; } = "{}";
        
        [NotMapped]
        public Dictionary<string, List<string>> AvailableSlots
        {
            get => JsonSerializer.Deserialize<Dictionary<string, List<string>>>(AvailableSlotsJson) ?? new();
            set => AvailableSlotsJson = JsonSerializer.Serialize(value);
        }
    }
}