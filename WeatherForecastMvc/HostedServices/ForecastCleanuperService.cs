internal interface IForecastCleanuperService
{
    Task CleanupPeriodically(uint cleanupPeriodSec, CancellationToken stoppingToken);
    void CleanupOnce();
}

internal class ForecastCleanuperService : IForecastCleanuperService
{
    private int executionCount = 0;
    private readonly ILogger _logger;
    private readonly ForecastContext _context;
    private readonly IConfiguration Configuration;

    public ForecastCleanuperService(ILogger<ForecastCleanuperService> logger, ForecastContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        Configuration = configuration;
    }

    public async Task CleanupPeriodically(uint cleanupPeriodSec, CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;
            _logger.LogInformation("ForecastCleanuperService is working. Count: {Count}", executionCount);
            CleanupOldRecords();
            await Task.Delay(TimeSpan.FromSeconds(cleanupPeriodSec), stoppingToken);
        }
    }

    public void CleanupOnce()
    {
        CleanupOldRecords();
    }

    private void CleanupOldRecords()
    {
        _logger.LogInformation($"Cleaning up old records. Time:{DateTime.Now.ToLongTimeString()}");

        if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:CalendarSpanDays"], out var oldRecordsCleanupDays))
        {
            oldRecordsCleanupDays = 10;
        }

        var oldRecords = _context.DayForecast
        .Where(f => f.Date < DateTime.Today.AddDays(-oldRecordsCleanupDays));

        if (!oldRecords.Any())
        {
            return;
        }

        string dates = String.Join(',', oldRecords.Select(r => r.Date.ToShortDateString()));
        _logger.LogWarning($"Removing records for dates:{dates}");

        _context.DayForecast.RemoveRange(oldRecords);
        _context.SaveChanges();
    }
}