using KitchenConnection.Models.Dispatcher;
using KitchenConnection.Models.HelperModels;
using KitchenConnection.Notifier.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace KitchenConnection.Notifier {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IModel _channel;
        private IConnection _connection;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, RabbitMqConfig rabbitMqConfig) {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _rabbitMqConfig = rabbitMqConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            stoppingToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory() {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.UserName,
                Password = _rabbitMqConfig.Password,
            };
            _connection = factory.CreateConnection();
            try {
                _channel = _connection.CreateModel();
                // consume messages as before

                // declare index the queue
                _channel.QueueDeclare(queue: "created-recipe-email", durable: false, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueDeclare(queue: "created-review-email", durable: false, exclusive: false, autoDelete: false, arguments: null);

                // create a consumer
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var dispatcher = new MessageDispatcher(_serviceProvider);

                    if (ea.RoutingKey == "created-recipe-email") {
                        var createdRecipe = JsonConvert.DeserializeObject<CreatedRecipe>(message)!;
                        await dispatcher.DispatchAsync(createdRecipe);
                    } else if (ea.RoutingKey == "created-review-email") {
                        var createdReview = JsonConvert.DeserializeObject<CreatedReview>(message)!;
                        await dispatcher.DispatchAsync(createdReview);
                    }

                };
                _channel.BasicConsume(queue: "created-recipe-email", autoAck: true, consumer: consumer);
                _channel.BasicConsume(queue: "created-review-email", autoAck: true, consumer: consumer);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error consuming message");
            }
        }
    }
}