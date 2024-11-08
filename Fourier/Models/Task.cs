namespace Fourier.Models;

public class Task
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; }
    public int Input { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int Progress { get; set; }
    public string Result { get; set; }
    public bool IsCancelled { get; set; }
}