using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TesteBackend.Application.Movies.Command.AddMovie;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.CommandHandlers.Movies;

public class AddMovieCommandHandler : IRequestHandler<AddMovieCommand, Guid>
{
    private readonly MongoDbContext _dbContext;
    private readonly ILogger<AddMovieCommandHandler> _logger;

    public AddMovieCommandHandler(MongoDbContext dbContext, ILogger<AddMovieCommandHandler> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid> Handle(AddMovieCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var builder = Builders<Movie>.Filter;
        var filter = builder.Eq(m => m.Title, request.Title) & builder.Eq(m => m.Director, request.Director) & builder.Eq(m => m.Year, request.Year);
        
        var movies = await _dbContext.GetCollection<Movie>("Movies").FindAsync(filter, cancellationToken: cancellationToken);
        var existingMovie = await movies.FirstOrDefaultAsync(cancellationToken);
        
        if (existingMovie != null)
        {
            throw new ExistingRecordException("This Movie has been added");
        }

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Director = request.Director,
            MainActor = request.MainActor,
            Runtime = request.Runtime,
            Year = request.Year,
            Actors = request.Actors,
            Genre = request.Genre,
            Rated = request.Rated,
            ProductionBudget = request.ProductionBudget,
            WorldwideGross = request.WorldwideGross,
            Synopsis = request.Synopsis,
            Awards = request.Awards,
            Soundtrack = request.Soundtrack,
            ProductionStudio = request.ProductionStudio,
            Writers = request.Writers,
            MarvelChronology = request.MarvelChronology,
            SpecialParticipations = request.SpecialParticipations,
            Reception = request.Reception,
            Curiosities = request.Curiosities
        };

        await _dbContext.GetCollection<Movie>("Movies").InsertOneAsync(movie, cancellationToken);

        _logger.LogInformation("Movie Inserted", movie);

        return movie.Id;
    }
}
