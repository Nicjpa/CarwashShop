using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Repositories
{
    public class ShopServiceRepository : IServiceRepository
    {
        private readonly CarWashDbContext _dbContext;

        public ShopServiceRepository(CarWashDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }

        public async Task<IQueryable<Service>> GetAllServices(string userName, FilterServices filters)
        {
            var entities = _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .Where(x => x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName)))
                .OrderBy(x => x.Id)
                .AsQueryable();

            if (filters.ServiceID != null)
            {
                entities = entities.Where(x => x.Id == filters.ServiceID);
            }
            else
            {
                if (filters.CarWashShopID != null)
                    entities = entities.Where(x => x.CarWashShops.Select(x => x.CarWashShopId).Any(x => x == filters.CarWashShopID));

                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    entities = entities.Where(x => x.CarWashShops.Any(x => x.CarWashShop.Name.ToUpper().Contains(filters.CarWashShopName.ToUpper())));

                if (!string.IsNullOrWhiteSpace(filters.ServiceName))
                    entities = entities.Where(x => x.Name.ToUpper().Contains(filters.ServiceName.ToUpper()));

                if (!string.IsNullOrWhiteSpace(filters.ServiceDescription))
                    entities = entities.Where(x => x.Description.ToUpper().Contains(filters.ServiceDescription.ToUpper()));

                if (filters.MinPrice != null)
                    entities = entities.Where(x => x.Price >= filters.MinPrice);

                if (filters.MaxPrice != null)
                    entities = entities.Where(x => x.Price <= filters.MaxPrice);
            }
            return entities;
        }

        public async Task<CarWashShopsServices> GetCarWashShopService(int id)
        {
            var entity = await _dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.CarWashShopId == id);

            return entity;
        }

        public async Task<Service> GetServiceByID(int id, string userName)
        {
            var entity = await _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => (x.Id == id) && x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName)));

            return entity;
        }

        public async Task<CarWashShop> GetShopToRemoveServiceByID(int id, string userName)
        {
            var entity = _dbContext.CarWashsShops
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefault(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName) && x.CarWashShopsServices.Select(x => x.ServiceId).Contains(id));

            return entity;
        }
    }
}
