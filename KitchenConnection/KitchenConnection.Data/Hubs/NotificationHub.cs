using KitchenConnection.DataLayer.Models.DTOs.Review;
using Microsoft.AspNetCore.SignalR;

namespace KitchenConnecition.DataLayer.Hubs
{
    public class NotificationHub:Hub
    {
    
        public void BroadcastReview(ReviewCreateDTO review)
        {
            Clients.All.SendAsync("Receivereview", review);
        }
    }
}
