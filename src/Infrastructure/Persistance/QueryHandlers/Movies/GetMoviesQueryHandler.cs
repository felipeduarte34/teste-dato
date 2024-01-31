using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using TesteBackend.Application.Movies.Query.GetMovies;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.QueryHandlers.Movies;

public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, PagedResult<Movie>>
{
    private readonly MongoDbContext _dbContext;

    public GetMoviesQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        
        // Criação do índice de texto
        var collection = _dbContext.GetCollection<Movie>("Movies");
        var indexKeys = Builders<Movie>.IndexKeys.Text("$**");
        collection.Indexes.CreateOne(new CreateIndexModel<Movie>(indexKeys));
    }

    public async Task<PagedResult<Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var filter = string.IsNullOrEmpty(request.GenericSearch) ? 
            Builders<Movie>.Filter.Empty : 
            Builders<Movie>.Filter.Text(request.GenericSearch);
        
        var movies = await _dbContext
            .GetCollection<Movie>("Movies")
            .GetPaged(request.Page, request.PageSize, filter);
        
        return movies;
    }
}
