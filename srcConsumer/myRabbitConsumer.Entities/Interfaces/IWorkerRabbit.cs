
namespace myRabbitConsumer.Entities.Interfaces;

public interface IWorkerRabbit
{
    Task ExecuteAsync(string queueName, CancellationToken stoppingToken);
}
