namespace WeatherForecastMvc.HostedServices
{
    public class TimedService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<TimedService> _logger;
        private Timer? _timer = null;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration Configuration;

        public TimedService(ILogger<TimedService> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration, IServiceProvider services)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            Configuration = configuration;
            Services = services;
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
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<ForecastCleanuperService>();
                scopedProcessingService.CleanupOnce();
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