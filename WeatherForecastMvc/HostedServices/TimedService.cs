namespace WeatherForecastMvc.HostedServices
{
    public class TimedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedService> _logger;
        private Timer? _timer = null;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration Configuration;


        public TimedService(ILogger<TimedService> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            Configuration = configuration;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:CleanupPeriodSec"], out var cleanupPeriodSec))
            {
                cleanupPeriodSec = 60;
            }
            _timer = new Timer(CleanupOldRecords, null, TimeSpan.Zero, TimeSpan.FromSeconds(cleanupPeriodSec));
            return Task.CompletedTask;
        }

        private void CleanupOldRecords(object? state)
        {
            _logger.LogInformation("Cleaning up old records.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ForecastContext>();

                if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:OldRecordsCleanupDays"], out var oldRecordsCleanupDays))
                {
                    oldRecordsCleanupDays = 10;
                }
                var oldRecords = context.DayForecast.Where(f => f.Date < DateTime.Today.AddDays(-oldRecordsCleanupDays)).ToArray();
                string dates = String.Join(',', oldRecords.Select(r => r.Date));
                _logger.LogInformation($"Removing records for dates:{dates}");
                context.DayForecast.RemoveRange(oldRecords);
                context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}