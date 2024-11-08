namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;

public interface ICancellationTokenRepository : IRepository<CancellationToken>
{
}

public class CancellationTokenRepository : Repository<CancellationToken>, ICancellationTokenRepository
{
    public CancellationTokenRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }
}