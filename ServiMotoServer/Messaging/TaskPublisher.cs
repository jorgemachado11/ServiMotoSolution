using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ServiMotoServer.Messaging.Dtos;

public class TaskPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public TaskPublisher()
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
    }

    public void PublishNewTask(TaskModelDto task)
    {
        var message = JsonConvert.SerializeObject(task);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "task_exchange",
                              routingKey: task.ServiceId, // Routing key is service ID
                              basicProperties: null,
                              body: body);
    }
}
