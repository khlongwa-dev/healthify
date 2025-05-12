namespace backend.DTOs
{
    public class UserLoginDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}