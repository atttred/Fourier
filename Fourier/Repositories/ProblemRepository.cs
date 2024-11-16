namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;
using Microsoft.EntityFrameworkCore;

public interface IProblemRepository : IRepository<Problem>
{
    Task<IEnumerable<Problem>> GetAll(Guid userId);
}

public class ProblemRepository : Repository<Problem>, IProblemRepository
{
    public ProblemRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Problem>> GetAll(Guid userId)
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .Where(task => task.UserId == userId)
            .ToListAsync();
    }
}