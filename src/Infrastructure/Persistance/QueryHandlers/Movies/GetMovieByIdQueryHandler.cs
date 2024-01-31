using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using TesteBackend.Application.Movies.Query.GetMovieById;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.QueryHandlers.Movies;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieQueryModel>
{
    private readonly MongoDbContext _dbContext;

    public GetMovieByIdQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<MovieQueryModel> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = Builders<Movie>.Filter;
        var filter = builder.Eq(m => m.Id, request.MovieId);

        var movieDb = await _dbContext.GetCollection<Movie>("Movies")
            .FindAsync(filter, cancellationToken: cancellationToken);
            
        var movie = await movieDb.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return new MovieQueryModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Director = movie.Director,
            Runtime = movie.Runtime,
            Year = movie.Year,
            Actors = movie.Actors,
            Genre = movie.Genre,
            Rated = movie.Rated,
            ProductionBudget = movie.ProductionBudget,
            WorldwideGross = movie.WorldwideGross,
            Synopsis = movie.Synopsis,
            Awards = movie.Awards,
            Soundtrack = movie.Soundtrack,
            ProductionStudio = movie.ProductionStudio,
            Writers = movie.Writers,
            MarvelChronology = movie.MarvelChronology,
            SpecialParticipations = movie.SpecialParticipations,
            Reception = movie.Reception,
            Curiosities = movie.Curiosities
        };
    }
}