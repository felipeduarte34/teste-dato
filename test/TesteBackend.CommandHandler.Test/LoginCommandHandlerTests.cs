using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.Users;
using TesteBackend.Domain.IRepositories;
using TesteBackend.Persistence.CommandHandlers.Users;
using TesteBackend.Persistence.Jwt;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TesteBackend.CommandHandler.Tests
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            // Arrange
            var userStore = new Mock<IUserStore<User>>();
            userStore.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    UserName = "testUserName",
                    Id = 123
                });

            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var jwtService = new Mock<IJwtService>();
            var refreshTokenRepository = new Mock<IRefreshTokenRepository>();

            // Act
            var commandHandler = new LoginCommandHandler(userManager, jwtService.Object, refreshTokenRepository.Object);

            // Assert
            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}
