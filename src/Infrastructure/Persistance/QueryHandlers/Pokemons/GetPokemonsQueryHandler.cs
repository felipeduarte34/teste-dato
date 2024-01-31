using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TesteBackend.Application.Pokemons.Query.GetPokemons;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Persistence.Db;
using TesteBackend.Persistence.QueryHandlers.Pokemons.Specification;

namespace TesteBackend.Persistence.QueryHandlers.Pokemons;

public class GetPokemonsQueryHandler : IRequestHandler<GetPokemonsQuery, PagedResult<Pokemon>>
{
    private readonly CleanArchReadOnlyDbContext _dbContext;

    public GetPokemonsQueryHandler(CleanArchReadOnlyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<PagedResult<Pokemon>> Handle(GetPokemonsQuery request, CancellationToken cancellationToken)
    {
        // var pokemons = await _dbContext.Set<Pokemon>().GetPaged(request.Page, request.PageSize);
        // return pokemons;
        var specification = new PokemonSpecification(request);
        var pokemons = await _dbContext.Set<Pokemon>().GetPaged(specification, request.Page, request.PageSize);
        return pokemons;
    }
}
