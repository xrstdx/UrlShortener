using Moq;
using UrlShortener.Application.DTOs.Auth;
using UrlShortener.Application.Helpers;
using UrlShortener.Application.Services;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Abstractions.Services;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Enums;
using UrlShortener.Domain.Exceptions;
using UrlShortener.Infrastructure.Contexts;
using Xunit;

namespace UrlShortener.UnitTests.ServiceTests
{
    public class AuthServiceTests : ServiceTestsBase
    {
        [Fact]
        public async void Register_ShouldThrowUserAlreadyExistsException()
        {
            // Arrange
            var context = CreateContext();
            await AddOrdinaryUser(context);

            var userRepository = new Mock<IUsersRepository>();
            userRepository.Setup(p => p.UserExistsByEmail(It.IsAny<string>())).ReturnsAsync(true);
            var jwtProvider = new Mock<IJwtProvider>();

            var authService = new AuthService(userRepository.Object, jwtProvider.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await authService.RegisterAsync(_registrationDTO));
        }

        [Fact]
        public async void Register_ShouldBeSuccessful()
        {
            // Arrange
            var context = CreateContext();

            var userRepository = new Mock<IUsersRepository>();
            userRepository.Setup(p => p.UserExistsByEmail(It.IsAny<string>())).ReturnsAsync(false);
            userRepository.Setup(p => p.CreateUserAsync(It.IsAny<User>())).Callback<User>(async p => 
            {
                context.Users.Add(p);
                await context.SaveChangesAsync();
            }).Returns(Task.CompletedTask);

            var jwtProvider = new Mock<IJwtProvider>();

            var authService = new AuthService(userRepository.Object, jwtProvider.Object);

            // Act

            await authService.RegisterAsync(_registrationDTO);

            // Assert

            var createdUser = context.Users.FirstOrDefault(p => p.Email == _registrationDTO.Email);

            Assert.NotNull(createdUser);
        }

        [Fact]
        public async void Login_ShouldThrowIncorrectCredentialsExceptionIfWrongEmail()
        {
            // Arrange
            var context = CreateContext();
            await AddOrdinaryUser(context);

            var userRepository = new Mock<IUsersRepository>();
            userRepository.Setup(p => p.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((string email) => context.Users.FirstOrDefault(u => u.Email == email));
            var jwtProvider = new Mock<IJwtProvider>();

            var authService = new AuthService(userRepository.Object, jwtProvider.Object);

            var loginDTOWithWrongEmail = _loginDTO with { Email = "test123@gmail.com" };

            // Act & Assert
            await Assert.ThrowsAsync<IncorrectCredentialsException>(async () => await authService.LoginAsync(loginDTOWithWrongEmail));
        }

        [Fact]
        public async void Login_ShouldThrowIncorrectCredentialsExceptionIfWrongPassword()
        {
            // Arrange
            var context = CreateContext();
            await AddOrdinaryUser(context);

            var userRepository = new Mock<IUsersRepository>();
            userRepository.Setup(p => p.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(_ordinaryUser);
            var jwtProvider = new Mock<IJwtProvider>();

            var authService = new AuthService(userRepository.Object, jwtProvider.Object);

            var loginDTOWithWrongPassword = _loginDTO with { Password = "wrong pass" };

            // Act & Assert
            await Assert.ThrowsAsync<IncorrectCredentialsException>(async () => await authService.LoginAsync(loginDTOWithWrongPassword));
        }

        [Fact]
        public async void Login_ShouldBeSuccessful()
        {
            // Arrange
            var context = CreateContext();
            await AddOrdinaryUser(context);

            var userRepository = new Mock<IUsersRepository>();
            userRepository.Setup(p => p.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(_ordinaryUser);
            var jwtProvider = new Mock<IJwtProvider>();
            jwtProvider.Setup(p => p.GenerateToken(_ordinaryUser)).Returns(_jwtToken);

            var authService = new AuthService(userRepository.Object, jwtProvider.Object);

            // Act

            var token = await authService.LoginAsync(_loginDTO);

            // Assert

            Assert.Equal(_jwtToken, token);
        }

        private async Task AddOrdinaryUser(UrlShortenerDbContext context)
        {
            await AddItems(context, _ordinaryUser);
        }

        private readonly RegistrationDTO _registrationDTO = new RegistrationDTO("full name", "test@gmail.com", "Password1234");

        private readonly LoginDTO _loginDTO = new LoginDTO("test@gmail.com", "Password1234");

        private readonly User _ordinaryUser = new User
        {
            Id = new Guid("6ee60e0f-c489-4880-89ac-96077beeb555"),
            FullName = "full name",
            Email = "test@gmail.com",
            PasswordHash = PasswordHelper.HashPassword("Password1234"),
            Role = Roles.Ordinary
        };

        private readonly string _jwtToken = "sometoken123";
    }
}