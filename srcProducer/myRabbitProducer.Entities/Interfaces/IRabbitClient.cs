
namespace myRabbitProducer.Entities.Interfaces;

public interface IRabbitClient
{
    Task PublishAsync<T>(T message);
}
