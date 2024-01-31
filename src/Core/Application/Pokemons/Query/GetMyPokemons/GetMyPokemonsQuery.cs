using MediatR;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Pokemons;

namespace TesteBackend.Application.Pokemons.Query.GetMyPokemons;

public class GetMyPokemonsQuery : IRequest<PagedResult<Pokemon>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? GenericSearch { get; set; }
    public int UserId { get; set; }
}
