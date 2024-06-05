using MotorcycleClient.Models;
using System;
using System.Text;
#if WINDOWS
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
#endif

namespace MotorcycleClient.Services.Notifications
{
    public class NotificationService : INotificationService
    {
#if WINDOWS
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public NotificationService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "guest",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "task_exchange", type: "direct", durable: false, autoDelete: false, arguments: null);
        }

        public void SubscribeToTaskNotifications(string serviceId, Action<TaskModel> onTaskReceived)
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "task_exchange", routingKey: serviceId);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var task = JsonConvert.DeserializeObject<TaskModel>(message);

                // Invoke the callback to handle the received task
                onTaskReceived(task);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
#endif
    }
}
