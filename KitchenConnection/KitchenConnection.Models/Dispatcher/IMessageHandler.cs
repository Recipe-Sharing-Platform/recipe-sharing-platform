namespace KitchenConnection.Models.Dispatcher;
public interface IMessageHandler<T> where T : IMessage {
    Task HandleAsync(T message);
}