using AutoMapper;
using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarWashShopAPI.Controllers
{
    [Route("api/OwnerServices")]
    [ApiController]
    public class OwnerServiceController : ControllerBase
    {
        //private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<OwnerServiceController> _logger;

        public OwnerServiceController(
            //CarWashDbContext dbContext, 
            IMapper mapper, 
            IServiceRepository serviceRepository, 
            ILogger<OwnerServiceController> logger)
        {
            //_dbContext = dbContext;
            _mapper = mapper;
            _serviceRepository = serviceRepository;
            _logger = logger;
        }



        //--1---------------------------------------- GET ALL SERVICES  -------------------------------------------

        [HttpGet("GetAllServices-OwnerSide", Name = "getAllServices-OwnerSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<ServiceViewWithShopAssigned>>> Get([FromQuery] FilterServices filters)
        {
            string userName = User.Identity.Name;

            var serviceEntities = await _serviceRepository.GetAllServices(userName, filters);

            if (serviceEntities == null || !serviceEntities.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetAllServices-OwnerSide' " +
                    $"/ no filtered results / '0' SERVICES FOUND ");

                return NotFound("There is no Service with specified filter parameters..");
            }
            var servicesPaginated = await _serviceRepository.Pagination(HttpContext, serviceEntities, filters.RecordsPerPage, new PaginationDTO { Page = filters.Page, RecordsPerPage = filters.RecordsPerPage });

            var allServicesView = _mapper.Map<List<ServiceViewWithShopAssigned>>(servicesPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetAllServices-OwnerSide' " +
                    $"/ results found / '{allServicesView.Count}' SERVICES FOUND ");

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
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewServiceToShop-OwnerSide' " +
                    $"/ no shop found, bad shop ID '{shopId}' / ADD SERVICE FAILED ");

                return NotFound($"There is no CarWashShop with ID: '{shopId}'..");
            }

            shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.Owner.UserName));
            if (!possibleUserNames.Contains(userName))
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewServiceToShop-OwnerSide' " +
                    $"/ invalid attempt to access foreign shop '{shopServiceEntity.CarWashShop.Name}' with ID '{shopId}' / ADD SERVICE FAILED ");

                return BadRequest($"You don't have access to the {shopServiceEntity.CarWashShop.Name} with ID: '{shopId}'..");
            }

            var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
            shopServiceEntity.Service = newServiceEntity;

            await _serviceRepository.AddService(shopServiceEntity);
            await _serviceRepository.Commit();

            var newServiceCreated = _mapper.Map<ServiceView>(newServiceEntity);

            _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewServiceToShop-OwnerSide' " +
                    $"/ new service added to the shop with ID '{shopId}' / ADDED SERVICE SUCCESSFULLY ");

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
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopService-OwnerSide' " +
                    $"/ no service found with ID '{serviceID}' / SERVICE UPDATE FAILED ");

                return NotFound($"You don't have any service with ID '{serviceID}'..");
            }

            serviceEntity = _mapper.Map(serviceUpdate, serviceEntity);

            await _serviceRepository.UpdateEntity(serviceEntity);
            await _serviceRepository.Commit();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopService-OwnerSide' " +
                   $"/ updated service with ID '{serviceID}' / SERVICE UPDATED SUCCESSFULLY ");

            return serviceView;
        }



        //--3--------------------------------------------------------- PATCH THE SERVICE --------------------------------------------------

        [HttpPatch("PatchShopService", Name = "patchShopService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<ServiceView>> Patch(int serviceID, [FromBody] JsonPatchDocument<ServiceCreationAndUpdate> serviceUpdate)
        {
            string userName = User.Identity.Name;

            if (serviceUpdate == null)
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopService-OwnerSide' " +
                    $"/ no info specified to patch the service with ID '{serviceID}' / PATCH SERVICE FAILED ");

                return BadRequest("You didn't specify which info do you want to patch.."); 
            }

            var serviceEntity = await _serviceRepository.GetServiceByID(serviceID, userName);

            if (serviceEntity == null)
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopService-OwnerSide' " +
                    $"/ no service found, bad ID '{serviceID}' / PATCH SERVICE FAILED ");

                return NotFound($"You don't have any service with ID '{serviceID}'..");
            }

            var serviceEntityPatch = _mapper.Map<ServiceCreationAndUpdate>(serviceEntity);
            
            serviceUpdate.ApplyTo(serviceEntityPatch, ModelState);

            if (!TryValidateModel(serviceEntityPatch))
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopService-OwnerSide' " +
                   $"/ bad input entry to patch service with ID '{serviceID}' / PATCH SERVICE FAILED ");
                return BadRequest("Check your patch inputs..");
            }

            _mapper.Map(serviceEntityPatch, serviceEntity);
            await _serviceRepository.Commit();

            var serviceView = _mapper.Map<ServiceView>(serviceEntity);

            _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopService-OwnerSide' " +
                  $"/ successfully patched service with ID '{serviceID}' / PATCH SERVICE SUCCESS ");

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
            {
                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveServiceFromShop-OwnerSide' " +
                  $"/ no service found with ID '{serviceID}' / REMOVE SERVICE FAILED ");

                return NotFound($"You don't have any service with ID '{serviceID}'..");
            }

            var service = shopEntity.CarWashShopsServices
                .Select(x => x.Service)
                .FirstOrDefault(x => x.Id == serviceID);

            int amountOfServicesInShop = 0;
            shopEntity.CarWashShopsServices.ForEach(x => { amountOfServicesInShop++; });

            string shopName = shopEntity.Name;

            if (amountOfServicesInShop == 1)
            {
                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveServiceFromShop-OwnerSide' " +
                  $"/ attempt to remove only shop service, ID '{serviceID}' / REMOVE SERVICE FAILED ");

                return BadRequest(new JsonResult($"You cannot delete the last and only existing service that you have in '{shopName}' CarWashShop.."));
            }

            await _serviceRepository.RemoveService(service);
            await _serviceRepository.Commit();

            _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveServiceFromShop-OwnerSide' " +
                  $"/ service '{service.Name}' has been removed from the shop '{shopName}' / SERVICE HAS BEEN SUCCESSFULLY REMOVED ");

            return Ok(new JsonResult($"You have successfully removed '{service.Name}' service from the {shopName} CarWashShop.."));
        }

    }
}
