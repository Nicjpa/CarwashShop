using CarWashShopAPI.Repositories.IRepositories;

namespace CarWashShopAPI.Services
{
    public class RadarService : IHostedService, IDisposable
    {
        private readonly IRadarRepository _radarRepository;
        private Timer _bookingTimer;
        private Timer _shopTimer;
        private Timer _ownerTimer;

        public RadarService(IRadarRepository radarRepository)
        {
            _radarRepository = radarRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _bookingTimer = new Timer(_radarRepository.DoBookingWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            _ownerTimer = new Timer(_radarRepository.DoOwnerWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            _shopTimer = new Timer(_radarRepository.DoShopWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _bookingTimer?.Change(Timeout.Infinite, 0);
            _shopTimer?.Change(Timeout.Infinite, 0);
            _ownerTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _bookingTimer?.Dispose();
            _shopTimer?.Dispose();
            _ownerTimer?.Dispose();
        }
    }
}
