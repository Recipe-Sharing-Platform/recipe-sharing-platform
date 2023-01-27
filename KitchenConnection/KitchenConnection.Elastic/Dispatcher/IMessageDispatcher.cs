using KitchenConnection.Elastic.Models;

namespace KitchenConnection.Elastic.Dispatcher; 
public interface IMessageDispatcher {
    Task DispatchAsync<T>(T message) where T : IMessage;
}