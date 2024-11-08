namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;

public interface ITaskRepository : IRepository<Problem>
{
}

public class ProblemRepository : Repository<Problem>, ITaskRepository
{
    public ProblemRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }
}