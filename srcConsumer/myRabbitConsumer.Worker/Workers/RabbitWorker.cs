
using myRabbitConsumer.Entities.Interfaces;

namespace myRabbitConsumer.Worker.Workers;

public class RabbitWorker(ILogger<RabbitWorker> logger, IRabbitClient rabbitClient) : IWorkerRabbit
{
    public async Task ExecuteAsync(string queueName, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await rabbitClient.ConsumeAsync(queueName);
            if (!string.IsNullOrWhiteSpace(message))
            {
                logger.LogInformation(" [x] Received message from queue {QueueName}: {Message}", queueName, message);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
