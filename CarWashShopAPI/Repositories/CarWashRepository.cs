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

        public CarWashRepository(CarWashDbContext dbContext, IMapper mapper)
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
                    carWashShop = carWashShop.Where(x => x.Name.Contains(filterDTO.CarWashName));

                if (!string.IsNullOrWhiteSpace(filterDTO.AdvertisingDescription))
                    carWashShop = carWashShop.Where(x => x.AdvertisingDescription.Contains(filterDTO.AdvertisingDescription));

                if (!string.IsNullOrWhiteSpace(filterDTO.ServiceNameOrDescription))
                    carWashShop = carWashShop
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(filterDTO.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(filterDTO.ServiceNameOrDescription)));

                if (filterDTO.MinimumAmountOfWashingUnits != null)
                    carWashShop = carWashShop.Where(x => x.AmountOfWashingUnits >= filterDTO.MinimumAmountOfWashingUnits);

                if (filterDTO.RequiredAndEarlierOpeningTime != null)
                    carWashShop = carWashShop.Where(x => x.OpeningTime <= filterDTO.RequiredAndEarlierOpeningTime);

                if (filterDTO.RequiredAndLaterClosingTime != null)
                    carWashShop = carWashShop.Where(x => x.ClosingTime >= filterDTO.RequiredAndLaterClosingTime);

                
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
            var entity = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Owner.UserName == userName && x.CarWashShopId == id);

            return entity.CarWashShop;
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

        
    }
}
