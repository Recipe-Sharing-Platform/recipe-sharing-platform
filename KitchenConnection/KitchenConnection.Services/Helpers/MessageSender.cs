

using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;

public class MessageSender {
    private readonly ConnectionFactory _factory;

    public MessageSender() {
        _factory = new ConnectionFactory() {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
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