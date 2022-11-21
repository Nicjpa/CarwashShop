using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface IConsumerRepository
    {
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
        public Task<IQueryable<CarWashShop>> GetAllShops(CarWashFilter filter);
        public Task<IQueryable<Service>> GetAllServices(FilterServices filter);
        public Task<IQueryable<Booking>> GetAllBookings(string userName, BookingFilters filter);
        public Task<string> GetUserID(string userName);
        public Task<CarWashShop> GetShopToBookService(BookingCreation bookingCreation);
        public Task<Booking> GetBookingByID(int id, string userName);
        public Task AddBooking(Booking booking);
        public Task DeleteBooking(Booking booking);
        public Task Commit();

    }
}
