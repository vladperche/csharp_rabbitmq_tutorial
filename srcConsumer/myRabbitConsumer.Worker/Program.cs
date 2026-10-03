
using Serilog;
using myRabbitConsumer.Worker.Workers;
using myRabbitConsumer.CrossCutting.DependencyInjection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSerilog();
builder.Services.AddServices();
builder.Services.AddScoped<RabbitWorker>();
builder.Services.AddHostedService<MainWorker>();

var host = builder.Build();

try
{
    Log.Information("Starting the service...");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The service terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
