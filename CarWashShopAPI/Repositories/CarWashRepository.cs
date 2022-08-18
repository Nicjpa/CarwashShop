using AutoMapper;
using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Repositories
{
    public class CarWashRepository : ICarWashRepository
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public CarWashRepository(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<CarWashShop> GetAllShopsInPossesion(string userName)
        {
            var CarWashShopOwners = _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .Include(x => x.Owner)
                .Where(x => x.Owner.UserName == userName)
                .AsQueryable();

            return CarWashShopOwners.Select(x => x.CarWashShop);
        }

        public async Task<IQueryable<CarWashShop>> QueryFilter(IQueryable<CarWashShop> carWashShop, CarWashFilter filterDTO)
        {
            if (filterDTO.Id != null)
            {
                carWashShop = carWashShop.Where(x => x.Id == filterDTO.Id);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filterDTO.Name))
                    carWashShop = carWashShop.Where(x => x.Name.Contains(filterDTO.Name));

                if (!string.IsNullOrWhiteSpace(filterDTO.AdvertisingDescription))
                    carWashShop = carWashShop.Where(x => x.AdvertisingDescription.Contains(filterDTO.AdvertisingDescription));

                if (filterDTO.MinimumAmountOfWashingUnits != null)
                    carWashShop = carWashShop.Where(x => x.AmountOfWashingUnits >= filterDTO.MinimumAmountOfWashingUnits);

                if (filterDTO.RequiredAndEarlierOpeningTime != null)
                    carWashShop = carWashShop.Where(x => x.OpeningTime <= filterDTO.RequiredAndEarlierOpeningTime);

                if (filterDTO.RequiredAndLaterClosingTime != null)
                    carWashShop = carWashShop.Where(x => x.ClosingTime >= filterDTO.RequiredAndLaterClosingTime);

                if (!string.IsNullOrWhiteSpace(filterDTO.ServiceNameOrDescription))
                    carWashShop = carWashShop
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(filterDTO.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(filterDTO.ServiceNameOrDescription)));
            }
            return carWashShop;
        }

        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }
    }
}
