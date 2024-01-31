using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TesteBackend.Application.Pokemons.Query.GetMyPokemons;
using TesteBackend.Application.Pokemons.Query.GetPokemons;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Domain.Entities.UsersPokemons;
using TesteBackend.Persistence.Db;

namespace TesteBackend.Persistence.QueryHandlers.Pokemons;

public class GetMyPokemonsQueryHandler : IRequestHandler<GetMyPokemonsQuery, PagedResult<Pokemon>>
{
    private readonly CleanArchReadOnlyDbContext _dbContext;

    public GetMyPokemonsQueryHandler(CleanArchReadOnlyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<PagedResult<Pokemon>> Handle(GetMyPokemonsQuery request, CancellationToken cancellationToken)
    {
        // Consulta na tabela UserPokemon para obter os Pokemons do usuário atual
        var userPokemons = _dbContext.Set<UserPokemon>()
            .Where(up => up.UserId == request.UserId);

        // Juntar os resultados com a tabela Pokemons para obter os dados completos de cada Pokémon
        var pokemons = await _dbContext.Set<Pokemon>()
            .Join(userPokemons, p => p.Id, up => up.PokemonId, (p, up) => p)
            .GetPaged(request.Page, request.PageSize);

        return pokemons;
    }
}
