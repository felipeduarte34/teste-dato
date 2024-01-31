using TesteBackend.Common.Exceptions;
using TesteBackend.Persistence.CommandHandlers.Movies;
using TesteBackend.Persistence.Db;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TesteBackend.CommandHandler.Tests
{
    public class AddMovieCommandHandlerTests
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            var mockService = new Mock<IService>();
            var dbContext = new Mock<MongoDbContext>(mockService.Object);
            // var dbContext = new Mock<MongoDbContext>();
            var logger = new Mock<ILogger<AddMovieCommandHandler>>();

            var commandHandler = new AddMovieCommandHandler(dbContext.Object, logger.Object);

            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}