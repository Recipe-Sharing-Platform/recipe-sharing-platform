using KitchenConnection.Elastic.Models;

namespace KitchenConnection.Elastic.MessageHandlers;
internal interface IMessageHandler<T> where T : IMessage
{
    Task HandleAsync(T message);
}