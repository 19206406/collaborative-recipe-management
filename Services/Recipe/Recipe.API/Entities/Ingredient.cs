namespace Recipe.API.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; } = default!; 
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!; 
        public int DisplayOrder { get; set; }
        public Recipe Recipes { get; set; } = new(); 
    }
}
