namespace Fourier.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string HashedPassword { get; set; }

    public DateTime CreatedAt { get; set; }
}