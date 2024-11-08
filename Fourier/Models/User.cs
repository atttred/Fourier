namespace Fourier.Models;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<Problem> Tasks { get; set; } = new List<Problem>();
}