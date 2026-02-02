namespace Recipe.API.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Difficulty { get; set; }
        public int Servings { get; set; }
        public string ImageUrl { get; set; } = default!;
        public decimal AverageRating { get; set; }
        public int RatingCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // relación 
        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<Step> Steps { get; set; } = new List<Step>(); 
        public ICollection<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();
    }
}
