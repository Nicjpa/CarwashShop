using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Repositories
{
    public class RadarRepository : IRadarRepository
    {
        private readonly IServiceProvider _serviceProvider;
        public RadarRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async void DoBookingWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var currentMoment = DateTime.Now;
                var dbContext = scope.ServiceProvider.GetRequiredService<CarWashDbContext>();

                var bookingsToCheck = await dbContext.Bookings.Where(x => !x.IsPaid).ToListAsync();

                var confirmedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Confirmed);
                var rejectedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Rejected);
                var pendingBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Pending);

                foreach (var booking in confirmedBookings)
                {
                    if (currentMoment > booking.ScheduledDateTime.AddHours(1))
                        booking.IsPaid = true;
                }

                foreach (var booking in pendingBookings)
                {
                    if (currentMoment > booking.DateCreated.AddMinutes(20))
                        booking.BookingStatus = BookingStatus.Confirmed;
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

                var RemovalRequests = await dbContext.ShopRemovalRequests.ToListAsync();

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
                            dbContext.ShopRemovalRequests.Remove(request);
                    }

                    if (approveCounter == ownerCount)
                    {
                        var approvedRequests = RemovalRequests.FindAll(x => x.CarWashShopId == shop.Id);
                        dbContext.ShopRemovalRequests.RemoveRange(approvedRequests);
                        dbContext.CarWashsShops.Remove(shop);
                        dbContext.Services.RemoveRange(shop.CarWashShopsServices.Select(x => x.Service));
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

                var allDisbandRequests = await dbContext.OwnerRemovalRequests.ToListAsync();

                foreach (var request in allDisbandRequests)
                {
                    if (request.IsApproved)
                    {
                        dbContext.CarWashShopsOwners.Remove(new CarWashShopsOwners()
                        {
                            CarWashShopId = request.CarWashShopId,
                            OwnerId = request.OwnerToBeRemovedId
                        });

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
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
