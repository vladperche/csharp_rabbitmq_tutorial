
namespace myRabbitConsumer.Entities.Interfaces;

public interface IRabbitClient
{
    Task<string> ConsumeAsync(string queueName);
}
