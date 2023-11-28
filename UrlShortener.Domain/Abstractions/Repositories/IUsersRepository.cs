using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Abstractions.Repositories;

public interface IUsersRepository
{
    Task CreateUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> UserExistsByEmail(string email);
    Task<User?> GetUserByIdAsync(Guid id);
}
