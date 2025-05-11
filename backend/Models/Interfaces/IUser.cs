namespace backend.Models.Interfaces
{
    public interface IUser
    {
        int Id { get; }
        string Email { get; }
        string Password { get; }
    }
}