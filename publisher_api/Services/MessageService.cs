using System;
using System.Text;
using RabbitMQ.Client;

namespace publisher_api.Services
{
    // define interface and service
    public interface IMessageService
    {
        bool Enqueue(string message);
    }

    public class MessageService : IMessageService
    {
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        public MessageService()
        {
            Console.WriteLine("about to connect to rabbit");

            _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }
        public bool Enqueue(string messageString)
        {
            var body = Encoding.UTF8.GetBytes("server processed " + messageString);
            _channel.BasicPublish(exchange: "",
                                routingKey: "hello",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
            return true;
        }
    }
}

