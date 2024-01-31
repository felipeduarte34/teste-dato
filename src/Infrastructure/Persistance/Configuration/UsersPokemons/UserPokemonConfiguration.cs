using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteBackend.Domain.Entities.UsersPokemons;

namespace TesteBackend.Persistence.Configuration.UsersPokemons;

public class UserPokemonConfiguration : IEntityTypeConfiguration<UserPokemon>
{
    public void Configure(EntityTypeBuilder<UserPokemon> builder)
    {

        builder.HasOne(up => up.User)
            .WithMany(u => u.UserPokemons)
            .HasForeignKey(up => up.UserId);

        builder.HasOne(up => up.Pokemon)
            .WithMany(p => p.UserPokemons)
            .HasForeignKey(up => up.PokemonId);
    }
}