
using myRabbitConsumer.Entities.Interfaces;
using myRabbitConsumer.Entities.Models;

namespace myRabbitConsumer.Worker.Workers;

public class MainWorker : BackgroundService
{
    private readonly ILogger<MainWorker> _logger;
    private readonly Dictionary<string, Task> _activeTasks = [];
    private readonly Dictionary<string, IWorkerRabbit> _workers = [];

    public MainWorker(ILogger<MainWorker> logger, RabbitWorker rabbitWorker)
    {
        _logger = logger;
        _workers.Add(typeof(ProductModel).Name, rabbitWorker);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(" [*] MainWorker running at: {time}", DateTimeOffset.Now);
            foreach (var workerKey in _workers.Keys)
            {
                bool existsTask = _activeTasks.TryGetValue(workerKey, out Task? activeTask);
                if(!existsTask || (activeTask?.IsCompleted ?? false))
                {
                    if(existsTask && (activeTask?.IsFaulted ?? false))
                    {
                        _logger.LogError(activeTask?.Exception, " [x] Task for worker {WorkerKey} has faulted.", workerKey);
                    }
                    _activeTasks[workerKey] = Task.Run(() => _workers[workerKey].ExecuteAsync(workerKey, stoppingToken), stoppingToken);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
