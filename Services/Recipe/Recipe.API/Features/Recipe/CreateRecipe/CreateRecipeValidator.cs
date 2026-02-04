using FluentValidation;

namespace Recipe.API.Features.Recipe.CreateRecipe
{
    public class CreateRecipeValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeValidator()
        {
            
        }
    }
}
