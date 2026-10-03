
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using myRabbitProducer.Entities.Interfaces;

namespace myRabbitProducer.Services.RabbitMQ;

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

        var factory = new ConnectionFactory() { 
            HostName = rabbitHost,
            Port = int.Parse(rabbitPort),
            UserName = rabbitUser,
            Password = rabbitPass
        };

        try
        {
            Task.Run(async() =>
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

    public async Task PublishAsync<T>(T message)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");
        }

        var queueName = typeof(T).Name;
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var bodyMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(bodyMessage);
        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
        _logger.LogInformation(" [x] Sent {Message} to queue {QueueName}", bodyMessage, queueName);
    }
}
