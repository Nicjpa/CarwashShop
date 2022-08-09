using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            shopCreation.CarWashShopsOwners = shopCreation.CarWashShopsOwners.ConvertAll(x => x.ToUpper());
            string user = User.Identity.Name.ToUpper();

            if (!shopCreation.CarWashShopsOwners.Contains(user))
            {
                shopCreation.CarWashShopsOwners.Add(user);
            }
            var carWashShopEntity = _mapper.Map<CarWashShop>(shopCreation);
            _dbContext.CarWashs.Add(carWashShopEntity);
            
            try
            {
                await _dbContext.SaveChangesAsync();
                carWashShopEntity.Services.ForEach(x => carWashShopEntity.CarWashShopsServices.Add(new CarWashShopsServices() { ServiceId = x.Id }));
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
