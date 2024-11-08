namespace Fourier.Models;

public class CancellationToken
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public bool IsCancelled { get; set; }

    public Task Task { get; set; } = null!;
}