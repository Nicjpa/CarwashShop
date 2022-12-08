using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface IServiceRepository
    {
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
        public Task<IQueryable<Service>> GetAllServices(string userName, FilterServices filters);
        public Task<CarWashShopsServices> GetCarWashShopService(int shopId);
        public Task<Service> GetServiceByID(int id, string userName);
        public Task<CarWashShop> GetShopToRemoveServiceByID(int id, string userName);
        public Task Commit();
        public Task AddService(CarWashShopsServices service);
        public Task RemoveService(Service service);
        public Task UpdateEntity<T>(T entity);
    }
}
