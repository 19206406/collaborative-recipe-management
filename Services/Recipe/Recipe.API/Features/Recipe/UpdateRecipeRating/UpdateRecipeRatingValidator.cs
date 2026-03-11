using FluentValidation;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingValidator : AbstractValidator<UpdateRecipeRatingCommand>
    {
        public UpdateRecipeRatingValidator()
        {
            RuleFor(x => x.NewAverage)
                .NotEmpty().WithMessage("El promedio no puede ser vacío")
                .InclusiveBetween(1, 5).WithMessage("El promedio debe estar entre 0 y 5");

            RuleFor(x => x.NewRatingCount)
                .NotEmpty().WithMessage("La cantidad de calificaciones no puede ser vacío")
                .GreaterThan(0).WithMessage("La cantidad de calificaciones debe de ser mayor a cero"); 
        }
    }
}
