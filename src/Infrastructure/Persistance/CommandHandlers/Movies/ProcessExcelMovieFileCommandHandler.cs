using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TesteBackend.Application.Movies.Command.AddMovie;
using TesteBackend.Application.Movies.Command.ProcessExcelMovieFile;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.HistoryImportFiles;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.CommandHandlers.Movies;

public class ProcessExcelMovieFileCommandHandler : IRequestHandler<ProcessExcelMovieFileCommand, Guid>
{
     private readonly MongoDbContext _dbContext;
    private readonly ILogger<ProcessExcelMovieFileCommandHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ProcessExcelMovieFileCommandHandler(
        MongoDbContext dbContext, 
        ILogger<ProcessExcelMovieFileCommandHandler> logger,
        IServiceProvider serviceProvider)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<Guid> Handle(ProcessExcelMovieFileCommand request, CancellationToken cancellationToken)
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
        backgroundService.SetFilePathAndType(request.FilePath, "Movies");
        await backgroundService.StartAsync(cancellationToken);

        return history.Id;    
    }

}