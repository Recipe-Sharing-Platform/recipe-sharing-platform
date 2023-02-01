namespace KitchenConnection.Models.Entities
{
    public class RecommendationScore:BaseEntity
    {        
        public Guid UserId { get; set; }
        public Guid TagId { get; set; }
        public int Score { get; set; }  
    }
}
