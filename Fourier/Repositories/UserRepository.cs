namespace Fourier.Repositories;
using Fourier.Data;
using Fourier.Models;
using Microsoft.EntityFrameworkCore;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByUserNameAsync(string username);
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(FourierDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User> GetByUserNameAsync(string username)
    {
        return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);
    }
}