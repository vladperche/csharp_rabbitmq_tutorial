
using Microsoft.Extensions.DependencyInjection;
using myRabbitProducer.Entities.Interfaces;
using myRabbitProducer.Services.RabbitMQ;

namespace myRabbitProducer.CrossCutting.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitClient, RabbitClient>();
        return services;
    }
}
