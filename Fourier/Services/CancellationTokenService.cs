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
    private readonly IProblemRepository _problemRepository;

    public CancellationTokenService(ICancellationTokenRepository cancellationTokenRepository, IProblemRepository problemService)
    {
        _cancellationTokenRepository = cancellationTokenRepository;
        _problemRepository = problemService;
    }

    public async Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId)
    {
        var cancellationToken = new CancellationToken { TaskId = problemId };
        return await _cancellationTokenRepository.AddAsync(cancellationToken);
    }

    public async Task CancelProblemAsync(Guid problemId)
    {
        var problem = await _problemRepository.GetByIdAsync(problemId);

        var token = await _cancellationTokenRepository.GetAllAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(t => t.TaskId == problemId));

        token!.IsCancelled = true;

        await _cancellationTokenRepository.UpdateAsync(token);

        if (problem != null)
        {
            problem.IsCancelled = true;
            problem.Status = "Cancelled";
            await _problemRepository.UpdateAsync(problem);
        }
    }
}