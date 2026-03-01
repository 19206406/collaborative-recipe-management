using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Mapster;
using Recipe.API.Entities;
using Recipe.API.Features.Recipe.CreateRecipe;
using Recipe.API.Repositories.IngredientRepository;
using Recipe.API.Repositories.RecipeRepository;
using Recipe.API.Repositories.StepRepository;
using Recipe.API.Repositories.TagRepository;

namespace Recipe.API.Features.Recipe.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : ICommandHandler<UpdateRecipeCommand, UpdateRecipeResponse>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IStepRepository _stepRepository;
        private readonly ITagRepository _tagRepository;

        public UpdateRecipeCommandHandler
            (IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository, 
            IStepRepository stepRepository, ITagRepository tagRepository)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
            _tagRepository = tagRepository;
        }

        public async Task<UpdateRecipeResponse> Handle(UpdateRecipeCommand command, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipe(command.Id);

            if (recipe is null)
                throw new NotFoundException("receta", command.Id);

            if (command.UserId != recipe.UserId)
                throw new UnauthorizedException("El usuario no está autorizado para realizar esta acción");

            var updatedRecipe = command.Recipe.Adapt<Entities.Recipe>();
            updatedRecipe.Id = command.Id;
            updatedRecipe.UserId = command.UserId; 
            var recipeUpdated = await _recipeRepository.UpdateRecipeOnly(updatedRecipe);

            // Ingredientes ----------- 

            // actualizar los ingredientes de forma automatica agregando actualizando y eliminando 
            var recipeIngredients = recipe.Ingredients.ToList();
            var updateIngredients = command.Ingredients;

            // remover los ingredientes no existentes 
            var ingredientsToDelete = recipeIngredients
                .Where(exist => !updateIngredients.Any(np => np.Id == exist.Id)).ToList();
            await _ingredientRepository.RemoveIngredients(ingredientsToDelete);

            // actualizar los ingredientes creados
            var ingredientsToUpdate = recipeIngredients
                .Where(exist => updateIngredients.Any(np => np.Id == exist.Id)).ToList(); 

            foreach (var ingredient in ingredientsToUpdate)
            {
                var map = updateIngredients.First(i => i.Id == ingredient.Id);
                ingredient.Name = map.Name;
                ingredient.Quantity = map.Quantity;
                ingredient.Unit = map.Unit;
                ingredient.DisplayOrder = map.DisplayOrder; 
            }
            await _ingredientRepository.UpdateIngredients(ingredientsToUpdate);

            // agregar preferencias 
            var newIngredients = updateIngredients
                .Where(ui => ui.Id == 0 || !recipeIngredients.Any(ri => ri.Id == ui.Id))
                .ToList();

            List<Ingredient> ingredients = newIngredients.Adapt<List<Ingredient>>();
            ingredients.ForEach(i => i.RecipeId = command.Id); 
            await _ingredientRepository.AddIngredients(ingredients);

            // Steps ----- 

            var recipeSteps = recipe.Steps.ToList();
            var updateSteps = command.Steps;

            // remover los pasos no existentes 
            var stepsToDelete = recipeSteps
                .Where(exist => !updateSteps.Any(us => us.Id == exist.Id)).ToList();
            await _stepRepository.RemoveSteps(stepsToDelete);

            // actualizar los steps 
            var stepsToUpdate = recipeSteps
                .Where(exist => updateSteps.Any(us => us.Id == exist.Id)).ToList(); 

            foreach (var step in stepsToUpdate)
            {
                var map = updateSteps.First(us => us.Id == step.Id);
                step.StepNumber = map.StepNumber;
                step.Instruction = map.Instruction; 
            }
            await _stepRepository.UpdateSteps(stepsToUpdate);

            // agregar pasos
            var newSteps = updateSteps
                .Where(us => us.Id == 0 || !recipeSteps.Any(rs => rs.Id == us.Id))
                .ToList();

            List<Step> steps = newSteps.Adapt<List<Step>>();
            steps.ForEach(s => s.RecipeId = command.Id); 
            await _stepRepository.AddSteps(steps);

            // Tags -------------------

            var recipeTags = recipe.RecipeTags.ToList();
            var updateTags = command.Ingredients.Take(3).ToList();

            recipeTags
                .Zip(updateTags, (recipeTag, updatedTag) =>
                {
                        recipeTag.Tag = updatedTag.Name;
                        return recipeTag;
                })
                .ToList();

            await _tagRepository.UpdateTags(recipeTags);

            var r = await _recipeRepository.GetRecipe(command.Id);

            var recipeUp = new ResponseRecipe(r.Id, r.UserId, r.Title, r.Description, r.PrepTimeMinutes,
                r.CookTimeMinutes, r.Difficulty, r.Servings, r.ImageUrl, r.AverageRating, r.RatingCount, r.CreatedAt);

            var ingredientsUp = r.Ingredients.Select(ing => new ResponseIngredient(ing.Id, ing.Name, ing.Quantity,
                ing.Unit, ing.DisplayOrder)).ToList();

            var stepsUp = r.Steps.Select(ste => new ResponseStep(ste.Id, ste.StepNumber, ste.Instruction)).ToList();

            var tagsUp = r.RecipeTags.Select(rt => new ResponseTag(rt.Id, rt.Tag)).ToList();

            return new UpdateRecipeResponse(recipeUp, ingredientsUp, stepsUp, tagsUp); 
        }
    }
}