using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using ServiMotoServer.Messaging.Dtos;

public class TaskSubscriber
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private string _serviceId;
    private string _queueName;

    public event Action<TaskModelDto> OnTaskReceived;

    public TaskSubscriber()
    {
        var factory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq",
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "guest"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "task_exchange", type: "direct", durable: false, autoDelete: false, arguments: null);

        _queueName = _channel.QueueDeclare().QueueName;
    }

    public void SubscribeToService(string serviceId)
    {
        if (!string.IsNullOrEmpty(_serviceId))
        {
            _channel.QueueUnbind(queue: _queueName, exchange: "task_exchange", routingKey: _serviceId);
        }

        _serviceId = serviceId;
        _channel.QueueBind(queue: _queueName, exchange: "task_exchange", routingKey: _serviceId);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var task = JsonConvert.DeserializeObject<TaskModelDto>(message);

            // Handle the received task
            OnTaskReceived?.Invoke(task);
        };
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
    }
}
