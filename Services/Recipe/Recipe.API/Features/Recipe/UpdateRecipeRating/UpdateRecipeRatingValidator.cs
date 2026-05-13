using FluentValidation;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingValidator : AbstractValidator<UpdateRecipeRatingCommand>
    {
        public UpdateRecipeRatingValidator()
        {
            RuleFor(x => x.NewAverage)
                .InclusiveBetween(1, 5).WithMessage("El promedio debe estar entre 0 y 5");

            RuleFor(x => x.NewRatingCount)
                .GreaterThan(0).WithMessage("La cantidad de calificaciones debe de ser mayor a cero"); 
        }
    }
}
