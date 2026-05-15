using BuildingBlocks.CQRS;
using Mapster;
using Recipe.API.Repositories.RecipeRepository;

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
            var recipe = command.Recipe;
            var ingredients = command.Ingredients.ToList();
            var steps = command.Steps.ToList();
            var tags = ingredients.Take(3).ToList(); 

            var newRecipe = new Entities.Recipe
            {
                UserId = command.UserId,
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
                    Name = i.Name.Trim().ToLower(),
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    DisplayOrder = i.DisplayOrder
                }).ToList(), 
                Steps = steps.Select((s, i) => new Entities.Step
                { 
                    StepNumber = s.StepNumber, 
                    Instruction = s.Instruction
                }).ToList(),
                RecipeTags = tags.Select(t => new Entities.RecipeTag
                {
                    Tag = t.Name.Trim().ToLower()
                }).ToList()
            };

            // TODO: NO SE ESTAN CREANDO LOS TAGS 
            var r = await _recipeRepository.AddRecipe(newRecipe);

            var recipeMap = r.Adapt<CreateRecipeResponse>();

            return recipeMap; 
        }
    }
}
