using KitchenConnection.Models.HelperModels;
using RabbitMQ.Client;

namespace KitchenConnection.Notifier {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IModel _channel;
        private IConnection _connection;
        public Worker(ILogger<Worker> logger) {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            stoppingToken.ThrowIfCancellationRequested();

        }
    }
}