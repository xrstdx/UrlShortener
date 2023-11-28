using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Contexts;

namespace UrlShortener.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UrlShortenerDbContext _dbContext;

    public UsersRepository(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    public async Task<bool> UserExistsByEmail(string email)
    {
        var emailExists = await _dbContext.Users.AnyAsync(u => u.Email == email);

        return emailExists;
    }
}
