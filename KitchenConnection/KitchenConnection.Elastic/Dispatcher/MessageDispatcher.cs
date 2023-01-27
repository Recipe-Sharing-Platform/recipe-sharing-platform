using KitchenConnection.Elastic.MessageHandlers;
using KitchenConnection.Elastic.Models;

namespace KitchenConnection.Elastic.Dispatcher;
public class MessageDispatcher : IMessageDispatcher {
    private readonly IServiceProvider _serviceProvider;
    public MessageDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    public async Task DispatchAsync<T>(T message) where T : IMessage {
        var handlers = _serviceProvider.GetServices<IMessageHandler<T>>();
        foreach (var handler in handlers) {
            await handler.HandleAsync(message);
        }
    }
}