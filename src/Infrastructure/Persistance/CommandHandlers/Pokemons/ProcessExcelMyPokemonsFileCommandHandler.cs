using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TesteBackend.Application.Pokemons.Command.ProcessExcelPokemonFile;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.HistoryImportFiles;

namespace TesteBackend.Persistence.CommandHandlers.Pokemons;

public class ProcessExcelMyPokemonsFileCommandHandler : IRequestHandler<ProcessExcelMyPokemonsFileCommand, Guid>
{
    private readonly MongoDbContext _dbContext;
    private readonly ILogger<ProcessExcelMyPokemonsFileCommandHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ProcessExcelMyPokemonsFileCommandHandler(
        MongoDbContext dbContext, 
        ILogger<ProcessExcelMyPokemonsFileCommandHandler> logger,
        IServiceProvider serviceProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<Guid> Handle(ProcessExcelMyPokemonsFileCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));
        
        var history = new HistoryImportFile()
        {
            Id = Guid.NewGuid(),
            FilePath = request.FilePath,
            FileName = request.FileName,
            Status = "CREATED",
            CreatedAt = DateTime.UtcNow
        };
        
        await _dbContext.GetCollection<HistoryImportFile>("HistoryImportFiles").InsertOneAsync(history, cancellationToken: cancellationToken);

        // Inicie o processamento do arquivo Excel em segundo plano
        var backgroundService = _serviceProvider.GetRequiredService<ProcessExcelFileBackgroundService>();
        backgroundService.SetUserId(request.UserId);
        backgroundService.SetFilePathAndType(request.FilePath, "Pokemons");
        await backgroundService.StartAsync(cancellationToken);

        return history.Id;    
    }

}