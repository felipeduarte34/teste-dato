using MediatR;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Pokemons;

namespace TesteBackend.Application.Pokemons.Query.GetPokemons;

public class GetPokemonsQuery : IRequest<PagedResult<Pokemon>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Name { get; set; }
    public int? MinHP { get; set; }
    public int? MaxHP { get; set; }
    public int? Speed { get; set; }
}
