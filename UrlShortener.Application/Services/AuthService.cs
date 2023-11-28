using Mapster;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.DTOs.Auth;
using UrlShortener.Application.Helpers;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Abstractions.Services;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Enums;
using UrlShortener.Domain.Exceptions;

namespace UrlShortener.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(IUsersRepository usersRepository, IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task RegisterAsync(RegistrationDTO model)
    {
        var isExists = await _usersRepository.UserExistsByEmail(model.Email);

        if (isExists)
            throw new UserAlreadyExistsException();

        var user = model.Adapt<User>();

        user.PasswordHash = PasswordHelper.HashPassword(model.Password);
        user.Role = Roles.Ordinary;

        await _usersRepository.CreateUserAsync(user);
    }

    public async Task<string> LoginAsync(LoginDTO model)
    {
        var user = await _usersRepository.GetUserByEmailAsync(model.Email);

        if (user is null || user.PasswordHash != PasswordHelper.HashPassword(model.Password))
            throw new IncorrectCredentialsException();

        string token = _jwtProvider.GenerateToken(user);

        return token;    
    }
}
