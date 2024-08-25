using RoomReservationSystem.Services.Interfaces;

namespace RoomReservationSystem.Services;

public class RoomAvailabilityHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceProvider _serviceProvider;

    public RoomAvailabilityHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(UpdateAvailability, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        return Task.CompletedTask;
    }

    private async void UpdateAvailability(object state)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
            await reservationService.UpdateStatus();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}