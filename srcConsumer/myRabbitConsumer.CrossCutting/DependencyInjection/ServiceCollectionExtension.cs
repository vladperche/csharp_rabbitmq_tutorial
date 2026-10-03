
using Microsoft.Extensions.DependencyInjection;
using myRabbitConsumer.Entities.Interfaces;
using myRabbitConsumer.Services.RabbitMQ;

namespace myRabbitConsumer.CrossCutting.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitClient, RabbitClient>();
        return services;
    }
}
