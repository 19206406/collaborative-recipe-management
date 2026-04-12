using FluentValidation;

namespace Recommendation.API.Features.Recommendation.RecommendationsByIngredients
{
    public class RecommendationsByIngredientsCommandValidator : AbstractValidator<RecommendationsByIngredientsCommand>
    {
        public RecommendationsByIngredientsCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El userId no puede ser vacío")
                .GreaterThan(0).WithMessage("El userId debe de ser mayor que cero");

            RuleFor(x => x.Ingredients)
                .NotEmpty().WithMessage("La lista de ingredientes para las recomendaciones no puede ser vacia")
                .Must(x => x.Count > 0).WithMessage("La lista debe de contener al menos 1 un ingrediente");
        }
    }
}
