using System.ComponentModel.DataAnnotations;
using backend.Models.Interfaces;

namespace backend.Models
{
    public class Admin : IUser
    {
        public int Id { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}