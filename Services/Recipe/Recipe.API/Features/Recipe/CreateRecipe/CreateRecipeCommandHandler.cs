using BuildingBlocks.CQRS;
using Recipe.API.Repositories;

namespace Recipe.API.Features.Recipe.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, CreateRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<CreateRecipeResponse> Handle(CreateRecipeCommand command, CancellationToken cancellationToken)
        {
            //TODO=validacion de que un usuario exista para luego ya que toco que conectar servicios

            var recipe = command.Recipe;
            var ingredients = command.Ingredients.ToList();
            var steps = command.Steps.ToList();
            var tags = ingredients.Take(3).ToList(); 

            var newRecipe = new Entities.Recipe
            {
                UserId = recipe.UserId,
                Title = recipe.Title,
                Description = recipe.Description,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CookTimeMinutes = recipe.CookTimeMinutes,
                Difficulty = recipe.Difficulty,
                Servings = recipe.Servings,
                ImageUrl = recipe.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Ingredients = ingredients.Select(i => new Entities.Ingredient
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    DisplayOrder = i.DisplayOrder
                }).ToList(), 
                Steps = steps.Select((s, i) => new Entities.Step
                { 
                    StepNumber = i + 1, 
                    Instruction = s.Instruction
                }).ToList(),
                RecipeTags = tags.Select(t => new Entities.RecipeTag
                {
                    Tag = t.Name
                }).ToList()
            };

            var r = await _recipeRepository.AddRecipe(newRecipe);

            var recipeCreated = new ResponseRecipe(r.Id, r.UserId, r.Title, r.Description, r.PrepTimeMinutes, 
                r.CookTimeMinutes, r.Difficulty, r.Servings, r.ImageUrl, r.AverageRating, r.RatingCount, r.CreatedAt);

            var ingredientsCreated = r.Ingredients.Select(ing => new ResponseIngredient(ing.Id, ing.Name, ing.Quantity,
                ing.Unit, ing.DisplayOrder)).ToList(); 

            var stepsCreated = r.Steps.Select(ste => new ResponseStep(ste.Id, ste.StepNumber, ste.Instruction)).ToList();

            var tagsCreated = r.RecipeTags.Select(rt => new ResponseTag(rt.Id, rt.Tag)).ToList();

            return new CreateRecipeResponse(recipeCreated, ingredientsCreated, stepsCreated, tagsCreated);  
        }
    }
}
