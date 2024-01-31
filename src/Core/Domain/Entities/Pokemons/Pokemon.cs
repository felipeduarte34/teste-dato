using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TesteBackend.Domain.Entities.UsersPokemons;

namespace TesteBackend.Domain.Entities.Pokemons;

public class Pokemon : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [Range(0, 1000)]
    public int Hp  { get; set; }
    
    [Range(0, 1000)]
    public int Attack { get; set; }
    
    [Range(0, 1000)]
    public int Defense { get; set; }
    
    [Range(0, 1000)]
    public int SpecialAttack { get; set; }
    
    [Range(0, 1000)]
    public int SpecialDefense { get; set; }
    
    [Range(0, 1000)]
    public int Speed { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<UserPokemon> UserPokemons { get; set; }
}