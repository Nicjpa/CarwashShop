using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Controllers
{
    [Route("api/Consumer")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConsumerController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllShopsFilteredOrByShopID", Name = "getAllShopsFilteredOrByShopID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> Get([FromQuery] CarWashFilter shopFilter)
        {
            var carShopsEntities = _dbContext.CarWashsShops
                .Include(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .AsQueryable();

            if (shopFilter.Id != null)
            {
                carShopsEntities = carShopsEntities.Where(x => x.Id == shopFilter.Id);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(shopFilter.Name))
                    carShopsEntities = carShopsEntities.Where(x => x.Name.Contains(shopFilter.Name));

                if (!string.IsNullOrWhiteSpace(shopFilter.AdvertisingDescription))
                    carShopsEntities = carShopsEntities.Where(x => x.AdvertisingDescription.Contains(shopFilter.AdvertisingDescription));

                if (shopFilter.MinimumAmountOfWashingUnits != null)
                    carShopsEntities = carShopsEntities.Where(x => x.AmountOfWashingUnits >= shopFilter.MinimumAmountOfWashingUnits);

                if (shopFilter.RequiredAndEarlierOpeningTime != null)
                    carShopsEntities = carShopsEntities.Where(x => x.OpeningTime <= shopFilter.RequiredAndEarlierOpeningTime);

                if (shopFilter.RequiredAndLaterClosingTime != null)
                    carShopsEntities = carShopsEntities.Where(x => x.ClosingTime >= shopFilter.RequiredAndLaterClosingTime);

                if (!string.IsNullOrWhiteSpace(shopFilter.ServiceNameOrDescription))
                    carShopsEntities = carShopsEntities
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(shopFilter.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(shopFilter.ServiceNameOrDescription)));
            }

            if (carShopsEntities == null || carShopsEntities.Count() == 0)
                return NotFound("There is no CarWashShop with specified filter parameters..");

            var shopsView = _mapper.Map<List<CarWashShopView>>(carShopsEntities);

            return Ok(shopsView);
        }
    }
}
