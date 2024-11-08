namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;

public interface IUserRepository : IRepository<User>
{
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }
}