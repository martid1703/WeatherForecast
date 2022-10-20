namespace WeatherForecastMvc.HostedServices
{
    public class HostedService : BackgroundService
    {
        public IServiceProvider Services { get; }
        private readonly ILogger<HostedService> _logger;
        private readonly IConfiguration Configuration;

        public HostedService(ILogger<HostedService> logger, IServiceProvider services, IConfiguration configuration)
        {
            _logger = logger;
            Services = services;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<ForecastCleanuperService>();
                if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:CleanupPeriodSec"], out var cleanupPeriodSec))
                {
                    cleanupPeriodSec = 60;
                }
                await scopedProcessingService.CleanupPeriodically(cleanupPeriodSec, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}