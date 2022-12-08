using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface ICarWashRepository
    {
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
        public Task<IQueryable<CarWashShop>> GetAllShopsInPossesion(string userName);
        public Task<IQueryable<CarWashShop>> QueryFilter(IQueryable<CarWashShop> carWashShop, CarWashFilter filterDTO);
        public Task<List<string>> GetOwnerIDs(CarWashShopCreation shopCreation);
        public Task<CarWashShop> GetShopByID(int id, string userName);
        public Task<CarWashShop> GetShopWithServicesByID(int id, string userName);
        public Task<string> GetLoggedInOwnerID(CarWashShop carWashShop, string userName);
        public Task<string> ConcatenateCoOwnerNames(CarWashShop carWashShop, string userName);
        public Task<List<ShopRemovalRequest>> CreateShopRemovalRequests(CarWashShop carWashShop, string userName, CarWashShopRemovalRequestCreation statement);
        public Task<bool> CheckIfRequestExist(int id);
        public Task Commit();
        public Task UpdateEntity<T>(T entity);
        public Task<bool> CheckCreateShopName(string shopName);
        public Task<bool> CheckEditShopName(string shopName, int shopId);
        public Task AddRangeOfServices(List<Service> services);
        public Task RemoveRangeOfServices(List<Service> services);
        public Task AddCarWashShop(CarWashShop shop);
        public Task RemoveCarWashShop(CarWashShop shop);
        public Task AddRangeOfShopRemovals(List<ShopRemovalRequest> shopRemovals, CarWashShop shop);
    }
}
