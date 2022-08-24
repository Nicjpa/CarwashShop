using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Repositories
{
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly CarWashDbContext _dbContext;

        public ConsumerRepository(CarWashDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }

        public async Task<IQueryable<CarWashShop>> GetAllShops(CarWashFilter filter)
        {
            var entities = _dbContext.CarWashsShops
                .Include(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .AsQueryable();

            if (filter.CarWashShopId != null)
            {
                entities
                    = entities.Where(x => x.Id == filter.CarWashShopId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filter.CarWashName))
                    entities = entities.Where(x => x.Name.Contains(filter.CarWashName));

                if (!string.IsNullOrWhiteSpace(filter.AdvertisingDescription))
                    entities = entities.Where(x => x.AdvertisingDescription.Contains(filter.AdvertisingDescription));

                if (!string.IsNullOrWhiteSpace(filter.ServiceNameOrDescription))
                    entities = entities
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(filter.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(filter.ServiceNameOrDescription)));

                if (filter.MinimumAmountOfWashingUnits != null)
                    entities = entities.Where(x => x.AmountOfWashingUnits >= filter.MinimumAmountOfWashingUnits);

                if (filter.RequiredAndEarlierOpeningTime != null)
                    entities = entities.Where(x => x.OpeningTime <= filter.RequiredAndEarlierOpeningTime);

                if (filter.RequiredAndLaterClosingTime != null)
                    entities = entities.Where(x => x.ClosingTime >= filter.RequiredAndLaterClosingTime);
            }
            return entities;
        }

        public async Task<IQueryable<Service>> GetAllServices(FilterServices filters)
        {
            var entities = _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (filters.ServiceID != null)
            {
                entities = entities.Where(x => x.Id == filters.ServiceID);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    entities = entities.Where(x => x.CarWashShops.Any(x => x.CarWashShop.Name.Contains(filters.CarWashShopName)));

                if (!string.IsNullOrWhiteSpace(filters.ServiceName))
                    entities = entities.Where(x => x.Name.Contains(filters.ServiceName));

                if (!string.IsNullOrWhiteSpace(filters.ServiceDescription))
                    entities = entities.Where(x => x.Description.Contains(filters.ServiceDescription));

                if (filters.MinPrice != null)
                    entities = entities.Where(x => x.Price >= filters.MinPrice);

                if (filters.MaxPrice != null)
                    entities = entities.Where(x => x.Price <= filters.MaxPrice);
            }
            return entities;
        }

        public async Task<IQueryable<Booking>> GetAllBookings(string userName, BookingFilters filter)
        {
            var bookingsEntity = _dbContext.Bookings
                .Include(x => x.CarWashShop)
                .Include(x => x.Service)
                .Include(x => x.Consumer)
                .Where(x => x.Consumer.UserName == userName)
                .OrderBy(x => x.ScheduledDateTime)
                .AsQueryable();

            if (filter.BookingID != null)
            {
                bookingsEntity = bookingsEntity.Where(x => x.Id == filter.BookingID);
            }
            else
            {
                if (filter.CarWashShopID != null)
                    bookingsEntity = bookingsEntity.Where(x => x.CarWashShopId == filter.CarWashShopID);

                if (filter.ServiceID != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ServiceId == filter.ServiceID);

                if (!string.IsNullOrWhiteSpace(filter.CarWashShopName))
                    bookingsEntity = bookingsEntity.Where(x => x.CarWashShop.Name == filter.CarWashShopName);

                if (!string.IsNullOrWhiteSpace(filter.ServiceName))
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Name == filter.ServiceName);

                if (filter.OnScheduledDate != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date == filter.OnScheduledDate);

                if (filter.ScheduledDatesBefore != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date < filter.ScheduledDatesBefore);

                if (filter.ScheduledDatesAfter != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date > filter.ScheduledDatesAfter);

                if (filter.AtScheduledHour != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour == filter.AtScheduledHour);

                if (filter.ScheduledHoursBefore != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour < filter.ScheduledHoursBefore);

                if (filter.ScheduledHoursAfter != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour > filter.ScheduledHoursAfter);

                if (filter.IsActiveBooking)
                    bookingsEntity = bookingsEntity.Where(x => !x.IsPaid);

                if (filter.BookingStatus != 0)
                    bookingsEntity = bookingsEntity.Where(x => x.BookingStatus == filter.BookingStatus);

                if (filter.Price != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price == filter.Price);

                if (filter.MinPrice != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price >= filter.MinPrice);

                if (filter.MaxPrice != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price <= filter.MaxPrice);
            }
            return bookingsEntity;
        }

        public async Task<string> GetUserID(string userName)
        {
            string userId = await _dbContext.CustomUsers.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();
            return userId;
        }

        public async Task<CarWashShop> GetShopToBookService(BookingCreation bookingCreation)
        {
            var entity = await _dbContext.CarWashsShops
                .Include(x => x.Bookings)
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == bookingCreation.CarWashShopId && x.CarWashShopsServices
                                    .Select(x => x.ServiceId)
                                    .Contains(bookingCreation.ServiceId));
            return entity;
        }

        public async Task<Booking> GetBookingByID(int id, string userName)
        {
            var entity = await _dbContext.Bookings
                .Include(x => x.Consumer)
                .FirstOrDefaultAsync(x => x.Id == id && x.Consumer.UserName == userName);

            return entity;
        }
    }
}
