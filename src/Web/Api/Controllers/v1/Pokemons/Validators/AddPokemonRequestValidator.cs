using FluentValidation;
using TesteBackend.Api.Controllers.v1.Movies.Requests;
using TesteBackend.Api.Controllers.v1.Pokemons.Requests;

namespace TesteBackend.Api.Controllers.v1.Pokemons.Validators;

public class AddPokemonRequestValidator: AbstractValidator<AddPokemonRequest>
{
    public AddPokemonRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

        RuleFor(x => x.Hp)
            .NotNull().NotEmpty().Must(hp => hp >= 1 && hp <= 1000).WithMessage("{PropertyName} must be between 1 and 1000");

        RuleFor(x => x.Attack)
            .NotNull().NotEmpty().Must(hp => hp >= 1 && hp <= 1000).WithMessage("{PropertyName} must be between 1 and 1000");

        RuleFor(x => x.Defense)
            .NotNull().NotEmpty().Must(hp => hp >= 1 && hp <= 1000).WithMessage("{PropertyName} must be between 1 and 1000");
        
        RuleFor(x => x.SpecialAttack)
            .NotNull().NotEmpty().Must(hp => hp >= 1 && hp <= 1000).WithMessage("{PropertyName} must be between 1 and 1000");
     
        RuleFor(x => x.SpecialDefense)
            .NotNull().NotEmpty().Must(hp => hp >= 1 && hp <= 1000).WithMessage("{PropertyName} must be between 1 and 1000");
    }
}
