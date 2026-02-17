namespace BuildingBlocks.Messaging.Events.RatingEvents
{
    public class RatingDeleteEvent
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
    }
}
