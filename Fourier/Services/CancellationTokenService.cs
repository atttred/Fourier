namespace Fourier.Services;
using Fourier.Repositories;
using Fourier.Models;
using System;
using System.Threading.Tasks;

public interface ICancellationTokenService
{
    Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId);
    Task CancelProblemAsync(Guid problemId);
    Task<CancellationToken> GetCancellationTokenAsync(Guid problemId);
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
        var cancellationToken = new CancellationToken { TaskId = problemId, IsCancelled = false };
        return await _cancellationTokenRepository.AddAsync(cancellationToken);
    }

    public async Task CancelProblemAsync(Guid problemId)
    {
        var problem = await _problemRepository.GetByIdAsync(problemId) ?? throw new ArgumentNullException($"The problem with {problemId} does not exist");
        var token = await _cancellationTokenRepository.GetByTaskId(problemId) ?? throw new ArgumentNullException($"The token for problem {problemId} does not exist");

        token.IsCancelled = true;
        problem.IsCancelled = true;
        problem.Status = "Cancelled";
        problem.CancellationToken = token;
        await _cancellationTokenRepository.UpdateAsync(token);
        await _problemRepository.UpdateAsync(problem);
    }

    public async Task<CancellationToken> GetCancellationTokenAsync(Guid problemId)
    {
        return await _cancellationTokenRepository.GetByTaskId(problemId);
    }
}