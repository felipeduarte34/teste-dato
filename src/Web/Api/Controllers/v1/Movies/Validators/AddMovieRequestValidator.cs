using FluentValidation;
using TesteBackend.Api.Controllers.v1.Movies.Requests;

namespace TesteBackend.Api.Controllers.v1.Movies.Validators;

public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
{
    public AddMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");

        RuleFor(x => x.Year)
            .NotNull().NotEmpty().GreaterThan(1970).WithMessage("{PropertyName} is not valid");
        
        RuleFor(x => x.Director)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is not valid");
        
    }
}
