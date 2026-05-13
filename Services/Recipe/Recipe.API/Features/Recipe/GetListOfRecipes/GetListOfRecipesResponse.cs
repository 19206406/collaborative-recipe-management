using BuildingBlocks.Pagination;

namespace Recipe.API.Features.Recipe.GetListOfRecipes
{
    public record ResponseRecipe(int Id, int UserId, string Title, string Description, int PrepTimeMinutes,
        int CookTimeMinutes, string Difficulty, int Servings, string ImageUrl, decimal AverageRating, int RatingCount, DateTime CreatedAt, DateTime UpdatedAt);
    public record GetListOfRecipesResponse(PaginatedResult<ResponseRecipe> Recipes); 
}
