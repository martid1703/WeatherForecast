internal interface IForecastCleanuperService
{
    Task DoWork(CancellationToken stoppingToken);
}

internal class ForecastCleanuperService : IForecastCleanuperService
{
    private int executionCount = 0;
    private readonly ILogger _logger;
    private readonly ForecastContext _context;

    public ForecastCleanuperService(ILogger<ForecastCleanuperService> logger, ForecastContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;
            _logger.LogInformation("ForecastCleanuperService is working. Count: {Count}", executionCount);
            CleanupOldRecords(5);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private void CleanupOldRecords(uint calendarSpanDays)
    {
        _logger.LogInformation($"Cleaning up old records. Time:{DateTime.Now.ToShortTimeString()}");

        var oldRecords = _context.DayForecast
        .Where(f => f.Date < DateTime.Today.AddDays(-calendarSpanDays))
        .ToArray();

        string dates = String.Join(',', oldRecords.Select(r => r.Date));
        _logger.LogInformation($"Removing records for dates:{dates}");

        _context.DayForecast.RemoveRange(oldRecords);
        _context.SaveChanges();
    }
}