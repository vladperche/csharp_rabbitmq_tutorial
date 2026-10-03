
using System.Text;

using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using myRabbitConsumer.Entities.Interfaces;

namespace myRabbitConsumer.Services.RabbitMQ;

public class RabbitClient : IRabbitClient
{
    private readonly ILogger<RabbitClient> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitClient(ILogger<RabbitClient> logger)
    {
        _logger = logger;
        var rabbitHost = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? "localhost";
        var rabbitPort = Environment.GetEnvironmentVariable("RABBIT_PORT") ?? "5672";
        var rabbitUser = Environment.GetEnvironmentVariable("RABBIT_USER") ?? "guest";
        var rabbitPass = Environment.GetEnvironmentVariable("RABBIT_PASS") ?? "guest";

        var factory = new ConnectionFactory()
        {
            HostName = rabbitHost,
            Port = int.Parse(rabbitPort),
            UserName = rabbitUser,
            Password = rabbitPass
        };

        try
        {
            Task.Run(async () =>
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }).Wait();
        }
        catch
        {
            throw;
        }
    }

    public async Task<string> ConsumeAsync(string queueName)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");
        }

        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _logger.LogInformation(" [*] Waiting for messages in queue {QueueName}.", queueName);

        var messageContent = string.Empty;
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            messageContent = Encoding.UTF8.GetString(body);
            _logger.LogInformation(" [x] Received {Message} from queue {QueueName}", messageContent, queueName);
        };

        await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

        if(string.IsNullOrWhiteSpace(messageContent))
        {
            _logger.LogInformation(" [*] No messages received from queue {QueueName}.", queueName);
        }
        _logger.LogInformation(" [*] End of messages consumption for queue {QueueName}.", queueName);

        return messageContent;
    }
}