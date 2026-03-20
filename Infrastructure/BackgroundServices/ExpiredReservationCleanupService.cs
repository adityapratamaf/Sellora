using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Domain.Interfaces.Orders;

public class ExpiredReservationCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredReservationCleanupService> _logger;

    public ExpiredReservationCleanupService(
        IServiceProvider serviceProvider,
        ILogger<ExpiredReservationCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var reservationRepo = scope.ServiceProvider
                    .GetRequiredService<IReservationRepository>();

                var now = DateTime.UtcNow;

                _logger.LogInformation("Running cleanup expired reservations...");

                await reservationRepo.CleanupExpiredAsync(now);
            }

            // crown job jalan tiap 1 menit
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}