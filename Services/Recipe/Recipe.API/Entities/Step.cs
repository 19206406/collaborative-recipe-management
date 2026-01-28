namespace Recipe.API.Entities
{
    public class Step
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = default!;
        public Recipe Recipe { get; set; } = new(); 
    }
}
