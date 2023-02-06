namespace KitchenConnection.Models.Dispatcher;
public interface IMessageDispatcher {
    Task DispatchAsync<T>(T message) where T : IMessage;
}