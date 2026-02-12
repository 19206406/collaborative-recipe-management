namespace Recommendation.API.Dtos
{
    public class IngredientMatchDto
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public decimal MatchPercentage { get; set; }
        public List<string> MatchedIngredients { get; set; }
        public List<string> MissingIngredients { get; set; }
    }
}
