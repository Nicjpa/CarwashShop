using AutoMapper;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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



        //--1---------------------------------------- GET ALL SERVICES WITH FILTERS OR BY 'ServiceId' ASSIGNED TO CAR WASH SHOPS IN OWNER'S POSSESSION -------------------------------------------

        [HttpGet("GetAllServicesFilteredOrByID", Name = "getAllServicesWithFiltersOrByID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<List<ServiceViewWithShopAssigned>>> Get([FromQuery] FilterServices filterServices)
        {
            string userName = User.Identity.Name;

            var serviceEntities = _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .Where(x => x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName))).AsQueryable();

            if(serviceEntities.Count() == 0 || serviceEntities == null)
                return NotFound("You didn't create any CarWashShop with services yet..");

            if (filterServices.ServiceID != null)
            {
                serviceEntities = serviceEntities.Where(x => x.Id == filterServices.ServiceID);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filterServices.CarWashShopName))
                    serviceEntities = serviceEntities.Where(x => x.CarWashShops.Any(x => x.CarWashShop.Name.Contains(filterServices.CarWashShopName)));

                if (!string.IsNullOrWhiteSpace(filterServices.Name))
                    serviceEntities = serviceEntities.Where(x => x.Name.Contains(filterServices.Name));

                if (!string.IsNullOrWhiteSpace(filterServices.Description))
                    serviceEntities = serviceEntities.Where(x => x.Description.Contains(filterServices.Description));

                if (filterServices.MinPrice != null)
                    serviceEntities = serviceEntities.Where(x => x.Price >= filterServices.MinPrice);

                if (filterServices.MaxPrice != null)
                    serviceEntities = serviceEntities.Where(x => x.Price <= filterServices.MaxPrice);
            }

            if (serviceEntities == null || serviceEntities.Count() == 0)
                return NotFound("There is no Service with specified filter parameters..");

            var allServicesView = _mapper.Map<List<ServiceViewWithShopAssigned>>(serviceEntities);

            return Ok(allServicesView);
        }



        //--2---------------------------------------- ADD NEW SERVICE TO EXISTING SHOP IN USER'S POSSESSION -------------------------------------------

        [HttpPost("AddNewServiceToShopByShopNameID", Name = "addNewServiceToShopByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> Post(string AddNewServiceToShopByShopNameOrShopID, [FromBody] ServiceCreationAndUpdate newServiceCreation)
        {
            string userName = User.Identity.Name;
            var possibleUserNames = new List<string>();

            bool isNotNumber = !int.TryParse(AddNewServiceToShopByShopNameOrShopID, out int id) && AddNewServiceToShopByShopNameOrShopID != "0";
            string type = isNotNumber ? "a name" : "ID";

            var shopServiceEntity = await _dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == AddNewServiceToShopByShopNameOrShopID.ToUpper());

            if (shopServiceEntity == null)
                return NotFound($"There is no CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopID}'..");


            shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.Owner.UserName));
            if (!possibleUserNames.Contains(userName))
                return BadRequest($"You don't have access to the CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopID}'..");

            var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
            shopServiceEntity.Service = newServiceEntity;

            _dbContext.CarWashShopsServices.Add(shopServiceEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully added a NEW service '{newServiceEntity.Name}' at your '{shopServiceEntity.CarWashShop.Name}' CarWashShop.");
        }



        //--3---------------------------------------- UPDATE SERVICE ON EXISTING SHOP IN USER'S POSSESSION BY 'ServiceID' -------------------------------------------

        [HttpPut("UpdateShopServiceByID", Name = "updateShopServiceByID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<ServiceView>> Put(int UpdateShopServiceByID, [FromBody] ServiceCreationAndUpdate serviceUpdate)
        {
            string userName = User.Identity.Name;

            var serviceEntity = await _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => (x.Id == UpdateShopServiceByID)
                                       && x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName)));

            if (serviceEntity == null)
                return NotFound($"You don't have any service with ID '{UpdateShopServiceByID}'..");

            serviceEntity = _mapper.Map(serviceUpdate, serviceEntity);
            _dbContext.Entry(serviceEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            return Ok(serviceView);
        }



        //--3---------------------------------------- PATCH SERVICE ON EXISTING SHOP IN USER'S POSSESSION BY 'ServiceID' -------------------------------------------

        [HttpPatch("PatchShopServiceByID", Name = "patchShopServiceByID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<ServiceView>> Patch(int PatchShopServiceByID, [FromBody] JsonPatchDocument<ServiceCreationAndUpdate> serviceUpdate)
        {
            string userName = User.Identity.Name;

            var serviceEntity = await _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => (x.Id == PatchShopServiceByID)
                                       && x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName)));

            if (serviceEntity == null)
                return NotFound($"You don't have any service with ID '{PatchShopServiceByID}'..");

            var serviceEntityPatch = _mapper.Map<ServiceCreationAndUpdate>(serviceEntity);

            serviceUpdate.ApplyTo(serviceEntityPatch, ModelState);

            if(!TryValidateModel(serviceEntityPatch)) 
                return BadRequest("Check your patch inputs..");

            _mapper.Map(serviceEntityPatch, serviceEntity);
            await _dbContext.SaveChangesAsync();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            return Ok(serviceView);
        }



        //--4---------------------------------------- DELETE SERVICE FROM EXISTING SHOP IN USER'S POSSESSION BY 'ServiceID' -------------------------------------------

        [HttpDelete("DeleteServiceByID", Name = "deleteServiceByID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> Delete(int DeleteServiceById)
        {
            string userName = User.Identity.Name;

            var serviceEntity = await _dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => (x.Id == DeleteServiceById)
                                       && x.CarWashShops.Any(x => x.CarWashShop.Owners.Any(x => x.Owner.UserName == userName)));

            if (serviceEntity == null)
                return NotFound($"You don't have any service with ID '{DeleteServiceById}'..");

            int amountOfServicesInShop = serviceEntity.CarWashShops.Select(x => x.CarWashShop.CarWashShopsServices.Count()).FirstOrDefault();
            string carWashShopName = serviceEntity.CarWashShops.Select(x => x.CarWashShop.Name).FirstOrDefault();

            if (amountOfServicesInShop == 1)
                return BadRequest($"You cannot delete the last and only existing service that you have in '{carWashShopName}' CarWashShop..");
            
            _dbContext.Services.Remove(serviceEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully removed '{serviceEntity.Name}' service from the {carWashShopName} CarWashShop..");
        }

    }
}
