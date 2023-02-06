using KitchenConnection.Models.DTOs.Review;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace KitchenConnection.DataLayer.Hubs {
    public class NotificationHub : Hub {
        public override Task OnConnectedAsync() {
            string userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) {
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            return base.OnConnectedAsync();
        }


        public void BroadcastReview(ReviewCreateDTO review) {
            Clients.All.SendAsync("Receivereview", review);
        }
    }
}
