using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TesteBackend.Application.Pokemons.Query.GetPokemonById;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Persistence.Db;

namespace TesteBackend.Persistence.QueryHandlers.Pokemons;

public class GetPokemonByIdQueryHandler: IRequestHandler<GetPokemonByIdQuery, PokemonQueryModel>
{
    private readonly CleanArchReadOnlyDbContext _dbContext;

    public GetPokemonByIdQueryHandler(CleanArchReadOnlyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<PokemonQueryModel> Handle(GetPokemonByIdQuery request, CancellationToken cancellationToken)
    {
        var existingPokemon = await _dbContext.Set<Pokemon>()
            .Where(a => a.Id == request.PokemonId)
            .Select(a => new PokemonQueryModel
            {
                Name = a.Name,
                Hp = a.Hp,
                Attack = a.Attack,
                Defense = a.Defense,
                SpecialAttack = a.SpecialAttack,
                SpecialDefense = a.SpecialDefense,
                Speed = a.Speed
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return existingPokemon;
    }
}