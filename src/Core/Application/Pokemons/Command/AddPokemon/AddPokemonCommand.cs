using System;
using MediatR;

namespace TesteBackend.Application.Pokemons.Command.AddPokemon;

public class AddPokemonCommand : IRequest<int>
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Speed { get; set; }
}