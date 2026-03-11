using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Recipe.API.Entities;
using Recipe.API.Repositories.IngredientRepository;
using Recipe.API.Repositories.RecipeRepository;
using Recipe.API.Repositories.StepRepository;
using Recipe.API.Repositories.TagRepository;
using Recipe.API.Repositories.UnitOfWork;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand, UpdateRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IStepRepository _stepRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRecipeCommandHandler
            (IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository, 
            IStepRepository stepRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateRecipeResponse> Handle(UpdateRecipeCommand command, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeOnly(command.Id);

            if (recipe is null)
                throw new NotFoundException("receta", command.Id);

            if (command.UserId != recipe.UserId)
                throw new UnauthorizedException("Usuario no autorizado para ejecutar esta acción"); 

            // actualizar solo la receta 
            recipe.Title = command.Recipe.Title;
            recipe.Description = command.Recipe.Description;
            recipe.PrepTimeMinutes = command.Recipe.PrepTimeMinutes;
            recipe.CookTimeMinutes = command.Recipe.CookTimeMinutes;
            recipe.Difficulty = command.Recipe.Difficulty;
            recipe.Servings = command.Recipe.Servings;
            recipe.ImageUrl = command.Recipe.ImageUrl;
            recipe.UpdatedAt = DateTime.UtcNow; 

            var ingredientsSummary = await ReplaceIngredientsAsync(command);
            var stepsSummary = await ReplaceStepsAsync(command);
            var tagsSummary = await ReplaceTagsAsync(command);

            await _unitOfWork.CommitAsync();

            return new UpdateRecipeResponse(recipe.Id, recipe.Title, recipe.Description, recipe.PrepTimeMinutes,
                recipe.CookTimeMinutes, recipe.Difficulty, recipe.Servings, recipe.ImageUrl, recipe.UpdatedAt,
                new RecipeUpdateSummary(ingredientsSummary.created, ingredientsSummary.updated, ingredientsSummary.deleted,
                stepsSummary.created, stepsSummary.updated, stepsSummary.deleted,
                tagsSummary.created, tagsSummary.updated, tagsSummary.deleted)); 

        }

        private async Task<CollectionSummary> ReplaceIngredientsAsync(UpdateRecipeCommand command)
        {
            var existing = await _ingredientRepository.GetIngredientsByIdRecipeAsync(command.Id);
            // ingredientes para actualizar ya que son los que tienen ids en su estructura 
            var incomingIds = command.Ingredients
                .Where(i => i.Id.HasValue) // solo se puede utilizar HasValue cuando la propiedad es nullable
                .Select(i => i.Id!.Value)
                .ToHashSet();

            // eliminar los que no estan en el command 
            var toDelete = existing.Where(e => !incomingIds.Contains(e.Id)).ToList();
            foreach (var item in toDelete)
                await _ingredientRepository.DeleteIngredientAsync(item.Id);

            // TODO: Implementar el CollectionSummary 
            int created = 0;
            int updated = 0; 

            // actualizar y crear 
            foreach (var ingredient in command.Ingredients)
            {
                if (ingredient.Id.HasValue)
                {
                    var exists = existing.FirstOrDefault(e => e.Id == ingredient.Id.Value);

                    exists.Name = ingredient.Name;
                    exists.Quantity = ingredient.Quantity;
                    exists.Unit = ingredient.Unit;
                    exists.DisplayOrder = ingredient.DisplayOrder;

                    await _ingredientRepository.UpdateIngredientAsync(exists);
                    updated++; 
                }
                else
                {
                    await _ingredientRepository.AddIngredientAsync(new Ingredient
                    {
                        RecipeId = command.Id, 
                        Name = ingredient.Name, 
                        Quantity = ingredient.Quantity, 
                        Unit = ingredient.Unit, 
                        DisplayOrder = ingredient.DisplayOrder, 
                    });
                    created++; 
                }
            }

            return new CollectionSummary(created, updated, toDelete.Count); 
        }

        private async Task<CollectionSummary> ReplaceStepsAsync(UpdateRecipeCommand command)
        {
            var existing = await _stepRepository.GetStepsByIdRecipeAsync(command.Id);

            var incomingIds = command.Steps
                .Where(s => s.Id.HasValue)
                .Select(s => s.Id!.Value)
                .ToHashSet();

            // eliminar 
            var toDelete = existing.Where(e => !incomingIds.Contains(e.Id)).ToList();
            foreach (var item in toDelete)
                await _stepRepository.DeleteStepAsync(item.Id);

            // actualizar y crear 
            int created = 0;
            int updated = 0; 

            foreach (var step in command.Steps)
            {
                if (step.Id.HasValue)
                {
                    var exists = existing.FirstOrDefault(e => e.Id == step.Id.Value);

                    exists.StepNumber = step.StepNumber;
                    exists.Instruction = step.Instruction;

                    await _stepRepository.UpdateStepAsync(exists);
                    updated++; 
                } 
                else
                {
                    await _stepRepository.AddStepAsync(new Step
                    {
                        RecipeId = command.Id, 
                        StepNumber = step.StepNumber, 
                        Instruction = step.Instruction
                    });
                    created++; 
                }
            }

            return new CollectionSummary(created, updated, toDelete.Count); 
        }

        private async Task<CollectionSummary> ReplaceTagsAsync(UpdateRecipeCommand command)
        {
            var existing = await _tagRepository.GetTagsByIdRecipeAsync(command.Id);
            var incomingIds = command.Tags
                .Where(t => t.Id.HasValue)
                .Select(t => t.Id!.Value)
                .ToHashSet();

            var toDelete = existing.Where(e => !incomingIds.Contains(e.Id)).ToList();

            foreach (var item in toDelete)
                await _tagRepository.DeleteTagAsync(item.Id);

            var created = 0;
            var updated = 0; 

            foreach (var tag in command.Tags)
            {
                if (tag.Id.HasValue)
                {
                    var exists = existing.FirstOrDefault(e => e.Id == tag.Id.Value);

                    exists.Tag = tag.Tag;

                    await _tagRepository.UpdateTagAsync(exists);
                    updated++; 
                }
                else
                {
                    await _tagRepository.AddTagAsync(new RecipeTag
                    {
                        RecipeId = command.Id, 
                        Tag = tag.Tag
                    });
                    created++; 
                }
            }

            return new CollectionSummary(created, updated, toDelete.Count); 
        }
    }
}