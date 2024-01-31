using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using PolyCache.Cache;
using TesteBackend.Application.Movies.Query.ReadMovieFromRedis;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.QueryHandlers.Movies;

public class ReadMovieFromRedisQueryHandler: IRequestHandler<ReadMovieFromRedisQuery, ReadMovieFromRedisResponse>
{
    private readonly MongoDbContext _dbContext;
    private readonly IStaticCacheManager _staticCacheManager;

    private const string _cachePrefix = "movie_";
    private const int _cacheExpiryTime = 2; //minitues

    public ReadMovieFromRedisQueryHandler(MongoDbContext dbContext,
        IStaticCacheManager staticCacheManager)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    }

    public async Task<ReadMovieFromRedisResponse> Handle(ReadMovieFromRedisQuery request, CancellationToken cancellationToken)
    {
        var movieId = request.MovieId;

        var result = await _staticCacheManager.GetWithExpireTimeAsync(
            new CacheKey(_cachePrefix + movieId),
            _cacheExpiryTime,
            async () => await GetMovieAsync());

        return result;

        async Task<ReadMovieFromRedisResponse> GetMovieAsync()
        {
            var builder = Builders<Movie>.Filter;
            var filter = builder.Eq(m => m.Id, request.MovieId);

            var movieDb = await _dbContext.GetCollection<Movie>("Movies")
                .FindAsync(filter, cancellationToken: cancellationToken);
            
            var movie = await movieDb.FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return new ReadMovieFromRedisResponse
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
}
