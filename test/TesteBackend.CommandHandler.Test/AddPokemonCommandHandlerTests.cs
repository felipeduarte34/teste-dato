using TesteBackend.Common.Exceptions;
using TesteBackend.Persistence.Db;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TesteBackend.Persistence.CommandHandlers.Pokemons;
using Xunit;

namespace TesteBackend.CommandHandler.Tests
{
    public class AddPokemonCommandHandlerTests
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            var dbContext = new Mock<CleanArchWriteDbContext>();
            var logger = new Mock<ILogger<AddPokemonCommandHandler>>();

            var commandHandler = new AddPokemonCommandHandler(dbContext.Object, logger.Object);

            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}