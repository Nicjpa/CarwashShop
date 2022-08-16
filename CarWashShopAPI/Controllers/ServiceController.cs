using AutoMapper;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarWashShopAPI.Controllers
{
    [Route("api/Service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public ServiceController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        //--1---------------------------------------- ADD NEW SERVICE TO EXISTING SHOP IN USER'S POSSESSION -------------------------------------------

        [HttpPost("AddNewServiceToShopByShopNameOrShopId", Name = "addNewServiceToShopByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(string AddNewServiceToShopByShopNameOrShopId, [FromBody] ServiceCreationView newServiceCreation)
        {
            string userName = User.Identity.Name;
            var possibleUserNames = new List<string>();

            bool isNotNumber = !int.TryParse(AddNewServiceToShopByShopNameOrShopId, out int id) && AddNewServiceToShopByShopNameOrShopId != "0";
            string type = isNotNumber ? "a name" : "ID";

            var shopServiceEntity = await _dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == AddNewServiceToShopByShopNameOrShopId.ToUpper());

            if (shopServiceEntity == null)
                return NotFound($"There is no CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");


            shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.Owner.UserName));
            if (!possibleUserNames.Contains(userName))
                return BadRequest($"You don't have access to the CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");

            var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
            shopServiceEntity.Service = newServiceEntity;

            _dbContext.CarWashShopsServices.Add(shopServiceEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully added a NEW service '{newServiceEntity.Name}' at your '{shopServiceEntity.CarWashShop.Name}' CarWashShop.");
        }



        //--1---------------------------------------- UPDATE SERVICE ON EXISTING SHOP IN USER'S POSSESSION -------------------------------------------

        //[HttpPut("UpdateShopServiceByShopNameOrShopId", Name = "addNewServiceToShopByShopNameOrShopId")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<ActionResult> Put(string AddNewServiceToShopByShopNameOrShopId, [FromBody] ServiceCreationView newServiceCreation)
        //{
        //    string userName = User.Identity.Name;
        //    var possibleUserNames = new List<string>();

        //    bool isNotNumber = !int.TryParse(AddNewServiceToShopByShopNameOrShopId, out int id) && AddNewServiceToShopByShopNameOrShopId != "0";
        //    string type = isNotNumber ? "a name" : "ID";

        //    var shopServiceEntity = await _dbContext.CarWashShopsServices
        //        .Include(x => x.CarWashShop)
        //        .ThenInclude(x => x.Owners)
        //        .ThenInclude(x => x.Owner)
        //        .FirstOrDefaultAsync(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == AddNewServiceToShopByShopNameOrShopId.ToUpper());

        //    if (shopServiceEntity == null)
        //        return NotFound($"There is no CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");


        //    shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.Owner.UserName));
        //    if (!possibleUserNames.Contains(userName))
        //        return BadRequest($"You don't have access to the CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");

        //    var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
        //    shopServiceEntity.Service = newServiceEntity;

        //    _dbContext.CarWashShopsServices.Add(shopServiceEntity);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok($"You have successfully added a NEW service '{newServiceEntity.Name}' at your '{shopServiceEntity.CarWashShop.Name}' CarWashShop.");
        //}
    }
}
