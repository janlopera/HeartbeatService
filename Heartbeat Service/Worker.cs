namespace Heartbeat_Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HTTPService _httpService;

    public Worker(ILogger<Worker> logger, HTTPService ser)
    {
        _logger = logger;
        _httpService = ser;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Log(LogLevel.Information, "Worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            _httpService.HeartBeat();
            _logger.Log(LogLevel.Information, "Heartbeat Sent");
            await Task.Delay(1000 * 60 * 5, stoppingToken);
        }
        _logger.Log(LogLevel.Information, "Worker stopped");
    }
}