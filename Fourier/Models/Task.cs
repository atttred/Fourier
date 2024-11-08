namespace Fourier.Models;

public class Task
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Input { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int Progress { get; set; }
    public string Result { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }

    public User User { get; set; } = null!;
    public CancellationToken? CancellationToken { get; set; } = null!;
}