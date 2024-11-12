namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;

public interface ICancellationTokenRepository : IRepository<CancellationToken>
{
    Task<CancellationToken> CreateCancellationTokenAsync(Guid problemId);
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
}