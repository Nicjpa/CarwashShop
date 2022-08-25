using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarWashShopAPI.Controllers
{
    [Route("api/OwnerServiceCRUD")]
    [ApiController]
    public class OwnerServiceCRUDController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;

        public OwnerServiceCRUDController(CarWashDbContext dbContext, IMapper mapper, IServiceRepository serviceRepository )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _serviceRepository = serviceRepository;
        }



        //--1---------------------------------------- GET ALL SERVICES  -------------------------------------------

        [HttpGet("GetAllServices-OwnerSide", Name = "getAllServices-OwnerSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<ServiceViewWithShopAssigned>>> Get([FromQuery] FilterServices filters)
        {
            string userName = User.Identity.Name;

            var serviceEntities = await _serviceRepository.GetAllServices(userName, filters);

            if (serviceEntities == null || !serviceEntities.Any())
                return NotFound("There is no Service with specified filter parameters..");

            var servicesPaginated = await _serviceRepository.Pagination(HttpContext, serviceEntities, filters.RecordsPerPage, filters.Pagination);

            var allServicesView = _mapper.Map<List<ServiceViewWithShopAssigned>>(servicesPaginated);

            return allServicesView;
        }



        //--2----------------------------------------------- ADD NEW SERVICE TO EXISTING SHOP ------------------------------------------------

        [HttpPost("AddNewServiceToShop", Name = "addNewServiceToShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<ServiceView>> Post(int shopId, [FromBody] ServiceCreationAndUpdate newServiceCreation)
        {
            string userName = User.Identity.Name;
            var possibleUserNames = new List<string>();

            var shopServiceEntity = await _serviceRepository.GetCarWashShopService(shopId);

            if (shopServiceEntity == null)
                return NotFound($"There is no CarWashShop with ID: '{shopId}'..");

            shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.Owner.UserName));
            if (!possibleUserNames.Contains(userName))
                return BadRequest($"You don't have access to the {shopServiceEntity.CarWashShop.Name} with ID: '{shopId}'..");

            var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
            shopServiceEntity.Service = newServiceEntity;

            _dbContext.CarWashShopsServices.Add(shopServiceEntity);
            await _dbContext.SaveChangesAsync();

            var newServiceCreated = _mapper.Map<ServiceView>(newServiceEntity);

            return newServiceCreated;
        }



        //--3----------------------------------------------------- UPDATE SERVICE BY ID -------------------------------------------

        [HttpPut("UpdateShopService", Name = "updateShopService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<ServiceView>> Put(int serviceID, [FromBody] ServiceCreationAndUpdate serviceUpdate)
        {
            string userName = User.Identity.Name;

            var serviceEntity = await _serviceRepository.GetServiceByID(serviceID, userName);

            if (serviceEntity == null)
                return NotFound($"You don't have any service with ID '{serviceID}'..");

            serviceEntity = _mapper.Map(serviceUpdate, serviceEntity);
            _dbContext.Entry(serviceEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            return serviceView;
        }



        //--3--------------------------------------------------------- PATCH THE SERVICE --------------------------------------------------

        [HttpPatch("PatchShopService", Name = "patchShopService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<ServiceView>> Patch(int serviceID, [FromBody] JsonPatchDocument<ServiceCreationAndUpdate> serviceUpdate)
        {
            string userName = User.Identity.Name;

            var serviceEntity = await _serviceRepository.GetServiceByID(serviceID, userName);

            if (serviceEntity == null)
                return NotFound($"You don't have any service with ID '{serviceID}'..");

            var serviceEntityPatch = _mapper.Map<ServiceCreationAndUpdate>(serviceEntity);
            
            serviceUpdate.ApplyTo(serviceEntityPatch, ModelState);

            if (!TryValidateModel(serviceEntityPatch))
                return BadRequest("Check your patch inputs..");

            _mapper.Map(serviceEntityPatch, serviceEntity);
            await _dbContext.SaveChangesAsync();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            return serviceView;
        }



        //--4-------------------------------------------------- REMOVE SERVICE FROM EXISTING SHOP -------------------------------------------

        [HttpDelete("RemoveService", Name = "removeService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> Delete(int serviceID)
        {
            string userName = User.Identity.Name;

            var shopEntity = await _serviceRepository.GetShopToRemoveServiceByID(serviceID, userName);

            if (shopEntity == null)
                return NotFound($"You don't have any service with ID '{serviceID}'..");

            var service = shopEntity.CarWashShopsServices
                .Select(x => x.Service)
                .FirstOrDefault(x => x.Id == serviceID);

            int amountOfServicesInShop = 0;
            shopEntity.CarWashShopsServices.ForEach(x => { amountOfServicesInShop++; });

            string shopName = shopEntity.Name;

            if (amountOfServicesInShop == 1)
                return BadRequest($"You cannot delete the last and only existing service that you have in '{shopName}' CarWashShop..");

            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully removed '{service.Name}' service from the {shopName} CarWashShop..");
        }

    }
}
