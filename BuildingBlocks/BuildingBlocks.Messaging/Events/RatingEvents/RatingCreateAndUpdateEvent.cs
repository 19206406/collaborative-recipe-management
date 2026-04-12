namespace BuildingBlocks.Messaging.Events.RatingEvents
{
    public class RatingCreateAndUpdateEvent
    {
        public int RatingId { get; set; }
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public int OldRating { get; set; }
        public string? Comment { get; set; }
        public bool IsToUpdate { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
