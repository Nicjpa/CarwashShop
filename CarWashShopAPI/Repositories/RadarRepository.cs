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

                var confirmedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Confirmed && !x.IsPaid && currentMoment > x.ScheduledDateTime.AddHours(1));
                var rejectedBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Rejected);
                var pendingBookings = bookingsToCheck.Where(x => x.BookingStatus == BookingStatus.Pending);

                var shops = confirmedBookings.Select(x => x.CarWashShop);
                var distinctShops = shops.Distinct();

                List<Transaction> transactions = new List<Transaction>();

                foreach (var booking in confirmedBookings)
                {                 
                    foreach(var shop in distinctShops)
                    { 
                        if(shop.Id == booking.CarWashShopId)
                        {
                            var transaction = new Transaction() 
                            { 
                                CarWashShopId = shop.Id, 
                                PaymentDay = booking.ScheduledDateTime, 
                                Amount = booking.Price 
                            };
                            transactions.Add(transaction);
                            shop.Revenue += booking.Price;
                            booking.IsPaid = true;
                            _logger.LogInformation($" / booking ID: '{booking.Id}' " +
                                $"/ booking charge: '{booking.Price}' / BOOKING CHARGE ");
                            break;
                        }                      
                    }    
                }
                dbContext.Transactions.AddRange(transactions);

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
      
                var allRemoveShopIDs = RemovalRequests
                    .Distinct()
                    .Select(x => x.CarWashShopId)
                    .ToList();
 
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
                    var currentMoment = DateTime.Now;
                    if (currentMoment > request.DateCreated.AddDays(7))
                    {
                        dbContext.OwnerRemovalRequests.Remove(request);

                        _logger.LogInformation($" / owner ID: '{request.OwnerToBeRemoved.Id}' " +
                            $"/ owner username: '{request.OwnerToBeRemoved.UserName}' / OWNER DISBAND REQUEST CANCELED AFTER 7 DATAS");
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
