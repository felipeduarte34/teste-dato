using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Domain.Entities.Users;

namespace TesteBackend.Domain.Entities.UsersPokemons;

public class UserPokemon : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public int PokemonId { get; set; }
    public Pokemon Pokemon { get; set; }
}