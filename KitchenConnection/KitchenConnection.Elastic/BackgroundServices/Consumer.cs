using KitchenConnection.DataLayer.Helpers;
using KitchenConnection.Elastic.Dispatcher;
using KitchenConnection.Elastic.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace KitchenConnection.Elastic.BackgroundServices;
public class Consumer : BackgroundService {
    private readonly ILogger<Consumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private IModel _channel; 
    private IConnection _connection;

    public Consumer(ILogger<Consumer> logger, IServiceProvider serviceProvider, RabbitMqConfig rabbitMqConfig) {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _rabbitMqConfig = rabbitMqConfig;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
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
            _channel.QueueDeclare(queue: "index-recipes", durable: false, exclusive: false, autoDelete: false, arguments: null);
            // declare the delete queue
            _channel.QueueDeclare(queue: "delete-recipes", durable: false, exclusive: false, autoDelete: false, arguments: null);
            // declare the update queue
            _channel.QueueDeclare(queue: "update-recipes", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // create a consumer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using (var fw = File.AppendText("messages.txt")) {
                    await fw.WriteAsync(message);
                }

                var dispatcher = new MessageDispatcher(_serviceProvider);

                if (ea.RoutingKey == "index-recipes") {
                    IndexRecipe indexRecipeMessage = JsonConvert.DeserializeObject<IndexRecipe>(message);
                    await dispatcher.DispatchAsync<IndexRecipe>(indexRecipeMessage);
                } else if(ea.RoutingKey == "delete-recipes") {
                    DeleteRecipe deleteRecipeMessage = JsonConvert.DeserializeObject<DeleteRecipe>(message);
                    await dispatcher.DispatchAsync<DeleteRecipe>(deleteRecipeMessage);
                } else if(ea.RoutingKey == "update-recipes") {
                    UpdateRecipe updateRecipeMessage = JsonConvert.DeserializeObject<UpdateRecipe>(message);
                    await dispatcher.DispatchAsync<UpdateRecipe>(updateRecipeMessage);
                }
                

            };
            _channel.BasicConsume(queue: "index-recipes", autoAck: true, consumer: consumer);
            _channel.BasicConsume(queue: "delete-recipes", autoAck: true, consumer: consumer);
            _channel.BasicConsume(queue: "update-recipes", autoAck: true, consumer: consumer);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error consuming message");
        }
        return Task.CompletedTask;
    }
}
