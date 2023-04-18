using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DDD.Shop.Common.BackgroundServices;

public abstract class RapidBackgroundService : BackgroundService
{
    private readonly ILogger<RapidBackgroundService> _logger;
    private readonly TimeSpan _interval;

    protected RapidBackgroundService(ILogger<RapidBackgroundService> logger, TimeSpan interval)
    {
        _logger = logger;
        _interval = interval;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await HandleAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing the background task.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    protected abstract Task HandleAsync(CancellationToken stoppingToken);
}