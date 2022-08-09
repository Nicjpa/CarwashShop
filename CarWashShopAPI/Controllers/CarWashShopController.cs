using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Controllers
{
    [Route("api/CarWashShop")]
    [ApiController]
    public class CarWashShopController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public CarWashShopController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("CreateCarWashShop", Name = "createCarWashShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] CarWashShopCreation shopCreation)
        {
            if (shopCreation.CarWashShopsOwners.Contains(null))
                return BadRequest("Each added shop owner must have name"); 

            shopCreation.CarWashShopsOwners = shopCreation.CarWashShopsOwners.ConvertAll(x => x.ToUpper());
            string user = User.Identity.Name.ToUpper();

            if (!shopCreation.CarWashShopsOwners.Contains(user))
                shopCreation.CarWashShopsOwners.Add(user);

            var carWashShopEntity = new CarWashShop();
            try
            {
                var services = _mapper.Map<List<Service>>(shopCreation.Services);
                _dbContext.Services.AddRange(services);
                await _dbContext.SaveChangesAsync();

                carWashShopEntity = _mapper.Map<CarWashShop>(shopCreation);
                services.ForEach(x => carWashShopEntity.CarWashShopsServices.Add(new CarWashShopsServices() { ServiceId = x.Id }));
                _dbContext.CarWashs.Add(carWashShopEntity);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if(ex.InnerException.Message.Contains("Cannot insert duplicate key row")) 
                    return BadRequest($"CarWashShop '{carWashShopEntity.Name}' already exsist..");

                return BadRequest("Your entries or entry formats are incorrect..");
            }

            return Ok($"You have successfully added NEW CarWashShop: '{carWashShopEntity.Name}'.");
        }

        [HttpGet("GetAllShopsInYourPossession", Name = "getAllShopsInYourPossession")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> Get()
        {
            string ownerId = User.Identity.Name;
            var carShopEntities = _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .Where(x => x.OwnerId == ownerId);

            var listCarWashShops = carShopEntities.Select(x => x.CarWashShop);

            var carWashShopsView = _mapper.Map<List<CarWashShopView>>(listCarWashShops);
            var carShopViewService = _mapper.Map<List<CarWashShopView>>(carShopEntities);

            for (int i = 0; i < carWashShopsView.Count; i++)
            {
                carWashShopsView[i].Services = carShopViewService[i].Services;
            }

            return Ok(carShopViewService);
        }
    }
}
