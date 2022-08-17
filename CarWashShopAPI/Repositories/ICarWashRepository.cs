using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories
{
    public interface ICarWashRepository
    {
        public IQueryable<CarWashShop> GetAllShopsInPossesion(string userName);
        public Task<IQueryable<CarWashShop>> FilterTheQuery(IQueryable<CarWashShop> carWashShop, CarWashFilter filterDTO);
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
    }
}
