using FluentValidation;

namespace Recipe.API.Features.Recipe.UpdateRecipeRating
{
    public class UpdateRecipeRatingValidator : AbstractValidator<UpdateRecipeRatingCommand>
    {
        public UpdateRecipeRatingValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("El rating debe estar entre 0 y 5");
        }
    }
}
