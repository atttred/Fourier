namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;
using Microsoft.EntityFrameworkCore;

public interface ICancellationTokenRepository : IRepository<CancellationToken>
{
    Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId);
    Task<CancellationToken> GetByTaskId(Guid problemId);
}

public class CancellationTokenRepository : Repository<CancellationToken>, ICancellationTokenRepository
{
    public CancellationTokenRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId)
    {
        var cancellationToken = new CancellationToken { TaskId = problemId };
        return await AddAsync(cancellationToken);
    }

    public async Task<CancellationToken> GetByTaskId(Guid problemId)
    {
        var token = await _dbContext.CancellationTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken => cancellationToken.TaskId == problemId);

        return token!;

        //return await GetAllAsync()
          //  .ContinueWith(task => task.Result.FirstOrDefault(cancellationToken => cancellationToken.TaskId == problemId)!);
    }
}