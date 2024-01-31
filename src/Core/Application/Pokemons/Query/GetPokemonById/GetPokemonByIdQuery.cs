using MediatR;

namespace TesteBackend.Application.Pokemons.Query.GetPokemonById;

public class GetPokemonByIdQuery : IRequest<PokemonQueryModel>
{
    public int PokemonId { get; set; }
}