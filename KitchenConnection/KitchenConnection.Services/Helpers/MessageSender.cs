using KitchenConnection.Models.HelperModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace KitchenConnection.BusinessLogic.Helpers;
public class MessageSender {
    private readonly ConnectionFactory _factory;

    public MessageSender(RabbitMqConfig rabbitMqConfig) {
        _factory = new ConnectionFactory() {
            HostName = rabbitMqConfig.HostName,
            UserName = rabbitMqConfig.UserName,
            Password = rabbitMqConfig.Password,
        };
    }

    public void SendMessage(object message, string queueName) {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        string messageString = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageString);
        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    }
}