namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.Models;
using System;
using System.Threading.Tasks;

public interface ICancellationTokenService
{
    Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId);
    Task CancelProblemAsync(Guid problemId);
}

public class CancellationTokenService : ICancellationTokenService
{
    private readonly ICancellationTokenRepository _cancellationTokenRepository;
    private readonly IProblemService _problemService;

    public CancellationTokenService(ICancellationTokenRepository cancellationTokenRepository, IProblemService problemService)
    {
        _cancellationTokenRepository = cancellationTokenRepository;
        _problemService = problemService;
    }

    public async Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId)
    {
        var cancellationToken = new CancellationToken { TaskId = problemId };
        return await _cancellationTokenRepository.AddAsync(cancellationToken);
    }

    public async Task CancelProblemAsync(Guid problemId)
    {
        var problem = await _problemService.GetTaskByIdAsync(problemId);
        if (problem != null)
        {
            problem.IsCancelled = true;
            await _problemService.UpdateTaskAsync(problem);
        }
    }
}