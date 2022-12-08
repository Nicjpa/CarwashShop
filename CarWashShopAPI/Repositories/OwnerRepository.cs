using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CarWashShopAPI.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly CarWashDbContext _dbContext;

        public OwnerRepository(CarWashDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }

        public async Task<List<ShopRemovalRequest>> GetShopRequestsToCancel(int id, string userName)
        {
            var entities = await _dbContext.ShopRemovalRequests
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .Where(x => x.CarWashShopId == id && x.CarWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .ToListAsync();

            return entities;
        }

        public async Task<List<ShopRemovalRequest>> GetShopRequestsToApprove(int id, string userName)
        {
            var entities = await _dbContext.ShopRemovalRequests
            .Include(x => x.Owner)
            .Include(x => x.CarWashShop)
            .Where(x => x.CarWashShopId == id && !x.IsApproved && x.Owner.UserName == userName)
            .ToListAsync();

            return entities;
        }

        public async Task<DisbandRequest> GetDisbandRequestToApprove(int id, string userName)
        {
            var entity = await _dbContext.OwnerRemovalRequests
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .Include(x => x.OwnerToBeRemoved)
                .FirstOrDefaultAsync(x => x.OwnerToBeRemoved.UserName == userName && x.CarWashShop.Id == id && !x.IsApproved);

            if(entity != null && entity.CarWashShop.Owners.Count > 1)
            {
                entity.IsApproved = true;
            }

            return entity;
        }

        public async Task RemoveDisbandRequest(DisbandRequest request)
        {
            var requestToDelete = request;
            _dbContext.OwnerRemovalRequests.Remove(requestToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingByID(BookingStatusSelection status, string userName)
        {
            var entity = await _dbContext.Bookings
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .FirstOrDefaultAsync(x => x.Id == status.BookingId && x.CarWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName));

            return entity;
        }

        public async Task<CarWashShop> GetShopToDisbandOwner(int id, string ownerName)
        {
            var entity = await _dbContext.CarWashsShops
                .Include(x => x.DisbandRequests)
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id && x.Owners.Select(x => x.Owner.UserName).Contains(ownerName));

            return entity;
        }

        public async Task<DisbandRequest> CreateDisbandRequest(DisbandRequestCreation statement, CarWashShop shop, string userName)
        {
            var removalRequest = new DisbandRequest()
            {
                RequestStatement = statement.RequestStatement,

                CarWashShopId = shop.Id,

                RequesterId = shop.Owners
                    .Where(x => x.Owner.UserName == userName).Select(x => x.OwnerId)
                    .FirstOrDefault(),

                OwnerToBeRemovedId = shop.Owners
                    .Where(x => x.Owner.UserName == statement.OwnerName.ToLower())
                    .Select(x => x.OwnerId)
                    .FirstOrDefault()
            };

            return removalRequest;
        }
        public async Task<CarWashShop> GetCarWashShopToAssignOwners(int id)
        {
            var entity = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task<List<CarWashShopsOwners>> AssignNewOwnersToTheShop(CarWashShop carWashShop, List<string> approvedOwnersIDs, List<string> CurrentOwnerUserIds)
        {
            var entities = new List<CarWashShopsOwners>();
            foreach (string userId in approvedOwnersIDs)
            {
                if (!CurrentOwnerUserIds.Contains(userId))
                {
                    entities.Add(new CarWashShopsOwners() { CarWashShopId = carWashShop.Id, OwnerId = userId });
                }
            }
            return entities;
        }
     
        private async Task<List<string>> GetPotentialOwnerIDs(CarWashShopOwnerAdd listOfNewOwners, List<string> currentOwnerList)
        {
            var potentialOwnerUserNames = listOfNewOwners.OwnerUserName.ConvertAll(x => x.ToLower());

            var potentialOwnersIDs = await _dbContext.Users
                .Where(x => potentialOwnerUserNames.Contains(x.UserName) && !currentOwnerList.Contains(x.UserName))
                .Select(x => x.Id)
                .ToListAsync();

            return potentialOwnersIDs;
        }

        public async Task<List<string>> GetApprovedOwnerIDs(CarWashShopOwnerAdd listOfNewOwners, List<string> currentOwnerList)
        {
            var potentialOwnersIDs = await GetPotentialOwnerIDs(listOfNewOwners, currentOwnerList);

            var approvedOwnerIDs = await _dbContext.UserClaims
                .Where(x => potentialOwnersIDs
                .Contains(x.UserId) && x.ClaimValue == "Owner")
                .Select(x => x.UserId)
                .ToListAsync();

            return approvedOwnerIDs;
        }

        public async Task<List<ShopIncome>> GetIncome(IncomeFilter filter, string userName)
        {
            var entities = await _dbContext.Income 
                .FromSqlInterpolated                                                           // DON'T SIMPLIFY 'filter.CalendarFormat', BECAUSE IT'S GONNA THROW SQL PARSING ERROR!
                ($"EXECUTE [dbo].[spGetShopIncome] @UserName={userName},@ShopId={filter.CarWashShopID},@DateType={filter.CalendarFormat.ToString()},@RecordsPerSearch={filter.RecordsPerSearch},@StartingDate={filter.StartingDate},@EndingDate={filter.EndingDate},@Year={filter.ForTheYear}")
                .ToListAsync();

            return entities;
        }

        public async Task<List<IncomeViewDays>> IncomeEntityMap2IncomeViewDays(List<ShopIncome> incomeEntities, IncomeFilter filter)
        {
            var incomeView = new List<IncomeViewDays>();

            incomeEntities.ToList().ForEach(x => incomeView.Add(new IncomeViewDays
            {
                CarWashShopId = x.CarWashShopId,
                CarWashShopName = x.CarWashShopName,
                Calendar = $"{filter.CalendarFormat} - {x.Calendar}",
                Date = x.Date.ToString("dddd, dd MMMM yyyy"),
                Income = x.Income.ToString("c")
            }));

            return incomeView;
        }

        public async Task<List<IncomeViewOther>> IncomeEntityMap2IncomeViewOther(List<ShopIncome> incomeEntities, IncomeFilter filter)
        {
            var monthName = new DateTimeFormatInfo();
            var incomeView = new List<IncomeViewOther>();

            incomeEntities.ToList().ForEach(x => incomeView.Add(new IncomeViewOther
            {
                CarWashShopId = x.CarWashShopId,
                CarWashShopName = x.CarWashShopName,
                Calendar = filter.CalendarFormat.ToString() == "Month" ? $"{filter.CalendarFormat} - {monthName.GetMonthName(x.Calendar)}" : $"{filter.CalendarFormat} - {x.Calendar}",
                Income = x.Income.ToString("c")
            }));

            return incomeView;
        }

        public async Task<IQueryable<CarWashShop>> GetShopsForRevenue(string userName, RevenueFilters revenueFilters)
        {
            var entities = _dbContext.CarWashsShops
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .Include(x => x.Owners)
                .Include(x => x.Bookings)
                .Where(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .OrderByDescending(x => x.Revenue)
                .AsQueryable();

            if (revenueFilters.ShopID != null)
                entities = entities.Where(x => x.Id == revenueFilters.ShopID);

            if (revenueFilters.ShopName != null)
                entities = entities.Where(x => x.Name.Contains(revenueFilters.ShopName));

            return entities;
        }

        public async Task<List<RevenueReportView>> CalculateRevenue(List<CarWashShop> carWashShops)
        {
            var allRevenueReports = new List<RevenueReportView>();

            foreach (var shop in carWashShops)
            {
                var revenueReport = new RevenueReportView() { CarWashShopID = shop.Id, CarWashShopName = shop.Name };

                foreach (var service in shop.CarWashShopsServices)
                {
                    var serviceRevenue = new ServiceRevenueView() { Id = service.ServiceId, ServiceName = service.Service.Name };

                    foreach (var booking in shop.Bookings)
                    {
                        if (booking.ServiceId == service.ServiceId && booking.IsPaid)
                        {
                            serviceRevenue.Revenue += booking.Price;
                            serviceRevenue.AmountOfBookings += 1;
                        }
                    }
                    revenueReport.Revenue = shop.Revenue;
                    revenueReport.TotalBookings += serviceRevenue.AmountOfBookings;
                    revenueReport.ByServicesRevenue.Add(serviceRevenue);
                }
                allRevenueReports.Add(revenueReport);
            }
            return allRevenueReports;
        }

        public async Task<List<ShopRemovalRequest>> GetShopRemovalRequests(string userName)
        {
            var entities = await _dbContext.ShopRemovalRequests
                .Include(x => x.Owner)
                .Include(x => x.CarWashShop)
                .Where(x => x.Owner.UserName == userName)
                .ToListAsync();

            return entities;
        }

        public async Task<List<DisbandRequest>> GetDisbandRequests(string userName)
        {
            var entities = await _dbContext.OwnerRemovalRequests
                .Include(x => x.Requester)
                .Include(x => x.CarWashShop)
                .Include(x => x.OwnerToBeRemoved)
                .Where(x => x.OwnerToBeRemoved.UserName == userName)
                .ToListAsync();

            return entities;
        }

        public async Task<IQueryable<Booking>> GetBookings(string userName, BookingFilters filters)
        {
            var entities = _dbContext.Bookings
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .Include(x => x.Service)
                .Include(x => x.Consumer)
                .OrderByDescending(x => x.ScheduledDateTime)
                .Where(x => x.CarWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .AsQueryable();

            if (filters.BookingID != null)
            {
                entities = entities.Where(x => x.Id == filters.BookingID);
            }
            else
            {
                if (filters.CarWashShopID != null)
                    entities = entities.Where(x => x.CarWashShopId == filters.CarWashShopID);

                if (filters.ServiceID != null)
                    entities = entities.Where(x => x.ServiceId == filters.ServiceID);

                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    entities = entities.Where(x => x.CarWashShop.Name.ToLower() == filters.CarWashShopName.ToLower());

                if (!string.IsNullOrWhiteSpace(filters.ShopAddress))
                    entities = entities.Where(x => x.CarWashShop.Address.ToLower().Contains(filters.ShopAddress.ToLower()));

                if (!string.IsNullOrWhiteSpace(filters.ServiceName))
                    entities = entities.Where(x => x.Service.Name.ToLower() == filters.ServiceName.ToLower());

                if (filters.OnScheduledDate != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Date == filters.OnScheduledDate);

                if (filters.ScheduledDatesBefore != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Date < filters.ScheduledDatesBefore);

                if (filters.ScheduledDatesAfter != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Date > filters.ScheduledDatesAfter);

                if (filters.AtScheduledHour != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Hour == filters.AtScheduledHour);

                if (filters.ScheduledHoursBefore != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Hour <= filters.ScheduledHoursBefore);

                if (filters.ScheduledHoursAfter != null)
                    entities = entities.Where(x => x.ScheduledDateTime.Hour >= filters.ScheduledHoursAfter);

                if (filters.IsActiveBooking)
                    entities = entities.Where(x => !x.IsPaid);

                if (filters.BookingStatus != 0)
                    entities = entities.Where(x => x.BookingStatus == filters.BookingStatus);

                if (filters.Price != null)
                    entities = entities.Where(x => x.Service.Price == filters.Price);

                if (filters.MinPrice != null)
                    entities = entities.Where(x => x.Service.Price >= filters.MinPrice);
                
                if (filters.MaxPrice != null)
                    entities = entities.Where(x => x.Service.Price <= filters.MaxPrice);
            }
            return entities;
        }

        public async Task<IQueryable<CarWashShop>> GetOwners(string userName, OwnersPerShopFilters filters)
        {
            var entities = _dbContext.CarWashsShops
                .Include(x => x.Owners).ThenInclude(x => x.Owner)
                .Where(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .AsQueryable();

            if (filters.CarWashShopId != null)
            {
                entities = entities.Where(x => x.Id == filters.CarWashShopId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    entities = entities.Where(x => x.Name == filters.CarWashShopName);
            }
            return entities;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEntity<T>(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task AddRangeOfOwners(List<CarWashShopsOwners> ownerList)
        {
            _dbContext.CarWashShopsOwners.AddRange(ownerList);
        }

        public async Task MakeDisbandRequest(DisbandRequest request)
        {
            _dbContext.OwnerRemovalRequests.Add(request);
        }

        public async Task CancelShopRemovalReq(List<ShopRemovalRequest> shopRemovalList, int shopID)
        {
            var shop = await _dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopID);
            shop.isInRemovalProcess = false;

            _dbContext.ShopRemovalRequests.RemoveRange(shopRemovalList);
            _dbContext.Entry(shop).State = EntityState.Modified;
        }

        public async Task RemoveMyselfFromShop(string userName, int shopID)
        {
            var owner = await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.UserName == userName);
            var shopOwner = await _dbContext.CarWashShopsOwners.FirstOrDefaultAsync(x => x.OwnerId == owner.Id && x.CarWashShopId == shopID);
            var removalRequest = await _dbContext.ShopRemovalRequests.FirstOrDefaultAsync(x => x.CarWashShopId == shopID && x.OwnerId == owner.Id);
            var ownerRemovalReqs = await _dbContext.OwnerRemovalRequests.Where(x => (x.CarWashShopId == shopID && x.RequesterId == owner.Id) || (x.CarWashShopId == shopID && x.OwnerToBeRemovedId == owner.Id)).ToListAsync();

            if(removalRequest != null)
            {
                _dbContext.ShopRemovalRequests.Remove(removalRequest);
            }

            if(ownerRemovalReqs != null && ownerRemovalReqs.Count > 0)
            {
                _dbContext.OwnerRemovalRequests.RemoveRange(ownerRemovalReqs);
            }
            _dbContext.CarWashShopsOwners.Remove(shopOwner);

            await _dbContext.SaveChangesAsync();
        }
    }
}
