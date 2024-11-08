namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;

public interface IProblemRepository : IRepository<Problem>
{
}

public class ProblemRepository : Repository<Problem>, IProblemRepository
{
    public ProblemRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }
}