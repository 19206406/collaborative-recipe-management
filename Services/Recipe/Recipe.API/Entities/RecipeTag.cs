namespace Recipe.API.Entities
{
    public class RecipeTag
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Tag { get; set; } = default!;
        public Recipe Recipe { get; set; } = null!; 
    }
}
