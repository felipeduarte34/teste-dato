using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TesteBackend.Application.Pokemons.Command.AddPokemon;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Persistence.Db;

namespace TesteBackend.Persistence.CommandHandlers.Pokemons;

public class AddPokemonCommandHandler: IRequestHandler<AddPokemonCommand, int>
{
    private readonly CleanArchWriteDbContext _dbContext;
    private readonly ILogger<AddPokemonCommandHandler> _logger;

    public AddPokemonCommandHandler(CleanArchWriteDbContext dbContext, ILogger<AddPokemonCommandHandler> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> Handle(AddPokemonCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var existingPokemon = await _dbContext.Set<Pokemon>().FirstOrDefaultAsync(a => a.Name == request.Name, cancellationToken);

        if (existingPokemon != null)
        {
            throw new ExistingRecordException("This Pokemon has been added");
        }

        var pokemon = new Pokemon
        {
            Name = request.Name,
            Hp = request.Hp,
            Attack = request.Attack,
            Defense = request.Defense,
            SpecialAttack = request.SpecialAttack,
            SpecialDefense = request.SpecialDefense,
            Speed = request.Speed
        };

        await _dbContext.Set<Pokemon>().AddAsync(pokemon, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pokemon Inserted");

        return pokemon.Id;
    }
}