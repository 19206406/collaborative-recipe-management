namespace BuildingBlocks.Models
{
    public class ProblemDetailsResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; 
        public int Status { get; set; }
        public string Instance { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new(); 
    }
}
