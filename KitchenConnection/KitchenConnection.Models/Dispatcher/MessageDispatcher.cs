using Microsoft.Extensions.DependencyInjection;

namespace KitchenConnection.Models.Dispatcher;
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