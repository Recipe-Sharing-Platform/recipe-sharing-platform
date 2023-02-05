using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Models.Entities;

namespace KitchenConnection.Notifier.Models; 
public class CreatedReview : IMessage {
    public User User { get; set; }
    public Recipe Recipe { get; set; }
    public Review Review { get; set; }
}
