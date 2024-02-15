namespace WorkerService;

public class WorkerService(ILogger<WorkerService> logger): BackgroundService
{
    private readonly ILogger<WorkerService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker is starting.");

        stoppingToken.Register(() => _logger.LogInformation("Worker is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker is doing background work.");

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        _logger.LogInformation("Worker has stopped.");
    }
}
