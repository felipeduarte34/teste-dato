using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteBackend.Domain.Entities.Pokemons;

namespace TesteBackend.Persistence.Configuration.Pokemons;

public class PokemonConfiguration : IEntityTypeConfiguration<Pokemon>
{
    public void Configure(EntityTypeBuilder<Pokemon> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Hp).IsRequired();
        builder.Property(p => p.Attack).IsRequired();
        builder.Property(p => p.Defense).IsRequired();
        builder.Property(p => p.SpecialAttack).IsRequired();
        builder.Property(p => p.SpecialDefense).IsRequired();
        builder.Property(p => p.Speed).IsRequired();
    }
}
