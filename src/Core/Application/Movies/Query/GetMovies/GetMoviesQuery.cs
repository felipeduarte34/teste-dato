using MediatR;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Application.Movies.Query.GetMovies;

public class GetMoviesQuery : IRequest<PagedResult<Movie>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? GenericSearch { get; set; }
}
