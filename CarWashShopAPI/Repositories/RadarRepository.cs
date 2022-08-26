using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Repositories
{
    public class RadarRepository : IRadarRepository
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RadarRepository> _logger;

        public RadarRepository(IServiceProvider serviceProvider, ILogger<RadarRepository> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async void DoBookingWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var currentMoment = DateTime.Now;
                var dbContext = scope.ServiceProvider.GetRequiredService<CarWashDbContext>();

                var bookingsToCheck = await dbContext.Bookings
                    .Include(x => x.Service)
                    .Include(x => x.CarWashShop)
                    .Where(x => !x.IsPaid)
                    .ToListAsync();

                var confirmedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Confirmed);
                var rejectedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Rejected);
                var pendingBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Pending);

                foreach (var booking in confirmedBookings)
                {
                    if (currentMoment > booking.ScheduledDateTime.AddHours(1))
                    {
                        booking.IsPaid = true;

                        _logger.LogInformation($" / booking ID: '{booking.Id}' " +
                            $"/ booking charge: '{booking.Service.Price}' / BOOKING CHARGE ");
                    }
                }

                foreach (var booking in pendingBookings)
                {
                    if (currentMoment > booking.DateCreated.AddMinutes(20))
                    {
                        booking.BookingStatus = BookingStatus.Confirmed;

                        _logger.LogInformation($" / booking ID: '{booking.Id}' / shop name: '{booking.CarWashShop.Name}' " +
                            $"/ booked service: '{booking.Service.Name}' / BOOKING CONFIRMED ");
                    }
                }
                dbContext.Bookings.RemoveRange(rejectedBookings);
                await dbContext.SaveChangesAsync();
            }
        }

        public async void DoShopWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var currentMoment = DateTime.Now;
                var dbContext = scope.ServiceProvider.GetRequiredService<CarWashDbContext>();

                var RemovalRequests = await dbContext.ShopRemovalRequests
                    .Include(x => x.Owner)
                    .ToListAsync();

                var allRemoveShopIDs = new List<int>();
                foreach (var request in RemovalRequests)
                {
                    if (!allRemoveShopIDs.Contains(request.CarWashShopId))
                        allRemoveShopIDs.Add(request.CarWashShopId);
                }

                var allShopsForRemoval = await dbContext.CarWashsShops
                    .Include(x => x.Owners)
                    .Include(x => x.CarWashShopsServices)
                    .ThenInclude(x => x.Service)
                    .Where(x => allRemoveShopIDs.Contains(x.Id))
                    .ToListAsync();

                foreach (var shop in allShopsForRemoval)
                {
                    int ownerCount = shop.Owners.Count - 1;
                    int approveCounter = 0;

                    foreach (var request in RemovalRequests)
                    {
                        if (request.CarWashShopId == shop.Id && request.IsApproved)
                            approveCounter++;

                        if (currentMoment > request.DateCreated.AddDays(7))
                        {
                            dbContext.ShopRemovalRequests.Remove(request);

                            _logger.LogInformation($" / shop removal request ID: '{request.Id}' " +
                                $"/ owner username: '{request.Owner.UserName}' / SHOP REMOVAL CANCELED AFTER 7 DAYS ");
                        }
                    }

                    if (approveCounter == ownerCount)
                    {
                        var approvedRequests = RemovalRequests.FindAll(x => x.CarWashShopId == shop.Id);
                        dbContext.ShopRemovalRequests.RemoveRange(approvedRequests);
                        dbContext.CarWashsShops.Remove(shop);
                        dbContext.Services.RemoveRange(shop.CarWashShopsServices.Select(x => x.Service));

                        _logger.LogInformation($" / removed shop ID: '{shop.Id}' " +
                            $"/ removed shop name: '{shop.Name}' / SHOP REMOVED SUCCESSFULLY ");
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async void DoOwnerWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CarWashDbContext>();

                var allDisbandRequests = await dbContext.OwnerRemovalRequests
                    .Include(x => x.OwnerToBeRemoved)
                    .ToListAsync();

                foreach (var request in allDisbandRequests)
                {
                    if (request.IsApproved)
                    {
                        dbContext.CarWashShopsOwners.Remove(new CarWashShopsOwners()
                        {
                            CarWashShopId = request.CarWashShopId,
                            OwnerId = request.OwnerToBeRemovedId
                        });
                        _logger.LogInformation($" / owner ID: '{request.OwnerToBeRemoved.Id}' " +
                            $"/ owner username: '{request.OwnerToBeRemoved.UserName}' / OWNER HAS BEEN DISBANDED ");

                        var shopRemovalRequests = await dbContext.ShopRemovalRequests
                            .FirstOrDefaultAsync(x => x.CarWashShopId == request.CarWashShopId && x.OwnerId == request.OwnerToBeRemovedId);

                        if (shopRemovalRequests != null)
                            dbContext.ShopRemovalRequests.Remove(shopRemovalRequests);

                        dbContext.OwnerRemovalRequests.Remove(request);
                    }
                    else
                    {
                        var currentMoment = DateTime.Now;
                        if (currentMoment > request.DateCreated.AddDays(7))
                        {
                            dbContext.OwnerRemovalRequests.Remove(request);

                            _logger.LogInformation($" / owner ID: '{request.OwnerToBeRemoved.Id}' " +
                                $"/ owner username: '{request.OwnerToBeRemoved.UserName}' / OWNER DISBAND REQUEST CANCELED AFTER 7 DATAS");
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
