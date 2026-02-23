namespace Recipe.API.Models
{
    public class RecipeSearchCriteria
    {
        public string? Title { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int? Difficulty { get; set; }
        public int? Servings { get; set; }
        public string? SortBy { get; set; } // "Title", "Difficulty", "PrepTimeMinutes", etc. 
        public bool SortDescending { get; set; }
        public List<string>? Tags { get; set; } // busquedad por preferencias 
    }
}
