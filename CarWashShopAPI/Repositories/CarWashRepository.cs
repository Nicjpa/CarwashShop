using AutoMapper;
using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Repositories
{
    public class CarWashRepository : ICarWashRepository
    {
        private readonly CarWashDbContext _dbContext;

        public CarWashRepository(CarWashDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }
        public async Task<IQueryable<CarWashShop>> QueryFilter(IQueryable<CarWashShop> carWashShop, CarWashFilter filterDTO)
        {
            if (filterDTO.CarWashShopId != null)
            {
                carWashShop = carWashShop.Where(x => x.Id == filterDTO.CarWashShopId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filterDTO.CarWashName))
                    carWashShop = carWashShop.Where(x => x.Name.ToLower().Contains(filterDTO.CarWashName.ToLower()));

                if (!string.IsNullOrWhiteSpace(filterDTO.Address))
                    carWashShop = carWashShop.Where(x => x.Address.ToLower().Contains(filterDTO.Address.ToLower()));

                if (!string.IsNullOrWhiteSpace(filterDTO.AdvertisingDescription))
                    carWashShop = carWashShop.Where(x => x.AdvertisingDescription.ToLower().Contains(filterDTO.AdvertisingDescription.ToLower()));

                if (!string.IsNullOrWhiteSpace(filterDTO.ServiceNameOrDescription))
                    carWashShop = carWashShop
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.ToLower().Contains(filterDTO.ServiceNameOrDescription.ToLower())
                                 || x.Service.Description.ToLower().Contains(filterDTO.ServiceNameOrDescription.ToLower())));

                if (filterDTO.MinimumAmountOfWashingUnits != null)
                    carWashShop = carWashShop.Where(x => x.AmountOfWashingUnits >= filterDTO.MinimumAmountOfWashingUnits);

                if (filterDTO.RequiredAndEarlierOpeningTime != null)
                    carWashShop = carWashShop.Where(x => x.OpeningTime >= filterDTO.RequiredAndEarlierOpeningTime);

                if (filterDTO.RequiredAndLaterClosingTime != null)
                    carWashShop = carWashShop.Where(x => x.ClosingTime <= filterDTO.RequiredAndLaterClosingTime);

                
            }
            return carWashShop;
        }

        public async Task<IQueryable<CarWashShop>> GetAllShopsInPossesion(string userName)
        {
            var entities = _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .Include(x => x.Owner)
                .Where(x => x.Owner.UserName == userName)
                .OrderByDescending(x => x.CarWashShop.Revenue)
                .AsQueryable();

            return entities.Select(x => x.CarWashShop);
        }

        public async Task<List<string>> GetOwnerIDs(CarWashShopCreation shopCreation)
        {
            var userIDs = await GetUserIDs(shopCreation);

            var ownerIDs = await _dbContext.UserClaims
                .Where(x => userIDs.Contains(x.UserId) && x.ClaimValue == "Owner")
                .Select(x => x.UserId)
                .ToListAsync();

            return ownerIDs;
        }

        private async Task<List<string>> GetUserIDs(CarWashShopCreation shopCreation)
        {
            var userIDs = await _dbContext.Users
                .Where(x => shopCreation.CarWashShopsOwners.Contains(x.UserName))
                .Select(x => x.Id)
                .ToListAsync();

            return userIDs;
        }

        public async Task<CarWashShop> GetShopByID(int id, string userName)
        {
            var entity = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Owners.Any(x => x.Owner.UserName == userName) && x.Id == id);

            return entity;
        }

        public async Task<CarWashShop> GetShopWithServicesByID(int id, string userName)
        {
            var entity = await _dbContext.CarWashsShops
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id && x.Owners.Select(x => x.Owner.UserName).Contains(userName));

            return entity;
        }

        public async Task<string> GetLoggedInOwnerID(CarWashShop carWashShop, string userName)
        {
            string userId = carWashShop.Owners
                .Where(x => x.Owner.UserName == userName).Select(x => x.Owner.Id)
                .FirstOrDefault();

            return userId;
        }

        public async Task<string> ConcatenateCoOwnerNames(CarWashShop carWashShop, string userName)
        {
            string otherOwners = string.Empty;
            carWashShop.Owners.ForEach(x => otherOwners += $"{x.Owner.UserName}, ");
            otherOwners = otherOwners.Replace($"{userName}, ", "");
            return otherOwners;
        }

        public async Task<List<ShopRemovalRequest>> CreateShopRemovalRequests(CarWashShop carWashShop, string userName, CarWashShopRemovalRequestCreation statement)
        {
            var OwnersIDs = carWashShop.Owners.Where(x => x.Owner.UserName != userName).Select(x => x.Owner.Id).ToList();
            var cwShopRemovalRequestList = new List<ShopRemovalRequest>();

            OwnersIDs.ForEach(x => cwShopRemovalRequestList.Add(new ShopRemovalRequest()
            {
                OwnerId = x,
                CarWashShopId = carWashShop.Id,
                RequestStatement = statement.RequestStatement
            }));

            return cwShopRemovalRequestList;
        }

        public async Task<bool> CheckIfRequestExist(int id)
        {
            bool doesExist = await _dbContext.ShopRemovalRequests
                .Include(x => x.CarWashShop)
                .AnyAsync(x => x.CarWashShop.Id == id);

            return doesExist;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEntity<T>(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> CheckEditShopName(string shopName, int shopId)
        {
            bool isLegit = true;
            var shop = await _dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Name == shopName);   

            if(shop != null)
            {
                isLegit = shop.Id == shopId;
            }

            return isLegit;
        }

        public async Task<bool> CheckCreateShopName(string shopName)
        {
            return await _dbContext.CarWashsShops.AnyAsync(x => x.Name == shopName);
        }

        public async Task AddRangeOfServices(List<Service> services)
        {
            _dbContext.Services.AddRange(services);
        }

        public async Task RemoveRangeOfServices(List<Service> services)
        {
            _dbContext.Services.RemoveRange(services);
        }

        public async Task AddCarWashShop(CarWashShop shop)
        {
            _dbContext.CarWashsShops.Add(shop);
        }

        public async Task RemoveCarWashShop(CarWashShop shop)
        {
            _dbContext.CarWashsShops.Remove(shop);
        }

        public async Task AddRangeOfShopRemovals(List<ShopRemovalRequest> shopRemovals, CarWashShop shop)
        {
            var shopForRemoval = shop;
            shopForRemoval.isInRemovalProcess = true;

            _dbContext.ShopRemovalRequests.AddRange(shopRemovals);
            _dbContext.Entry(shopForRemoval).State = EntityState.Modified;
        }

       
    }
}
