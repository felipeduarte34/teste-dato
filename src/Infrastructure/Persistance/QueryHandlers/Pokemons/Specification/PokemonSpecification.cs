using TesteBackend.Application.Pokemons.Query.GetPokemons;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Common.Utilities;

namespace TesteBackend.Persistence.QueryHandlers.Pokemons.Specification;

public class PokemonSpecification : Specification<Pokemon>
{
    public PokemonSpecification(GetPokemonsQuery request)
    {
        if (!string.IsNullOrEmpty(request.Name))
        {
            Add(p => p.Name.Contains(request.Name));
        }

        if (request.MinHP.HasValue)
        {
            Add(p => p.Hp >= request.MinHP.Value);
        }

        if (request.MaxHP.HasValue)
        {
            Add(p => p.Hp <= request.MaxHP.Value);
        }

        if (request.Speed.HasValue)
        {
            Add(p => p.Speed == request.Speed.Value);
        }
    }
}