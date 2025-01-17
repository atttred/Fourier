﻿namespace Fourier.Models;

public class CancellationToken
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public bool IsCancelled { get; set; } = false;

    public Problem Task { get; set; } = null!;
}