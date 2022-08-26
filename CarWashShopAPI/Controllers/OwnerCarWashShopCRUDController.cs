using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarWashShopAPI.Controllers
{
    [Route("api/OwnerCarWashShopCRUD")]
    [ApiController]
    public class OwnerCarWashShopCRUDController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICarWashRepository _carWashShopRepository;
        private readonly ILogger<OwnerCarWashShopCRUDController> _logger;

        public OwnerCarWashShopCRUDController
            (
            CarWashDbContext dbContext, 
            IMapper mapper, 
            ICarWashRepository carWashShopRepository,
            ILogger<OwnerCarWashShopCRUDController> logger
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _carWashShopRepository = carWashShopRepository;
            _logger = logger;
        }



        //--1------------------------------ GET ALL EXISTING SHOPS IN POSSESSION ---------------------- 

        [HttpGet("GetAllShops-OwnersSide", Name = "getAllShops-OwnersSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<CarWashShopView>>> Get([FromQuery] CarWashFilter shopFilter)
        {
            string userName = User.Identity.Name;

            var carWashShops = await _carWashShopRepository.GetAllShopsInPossesion(User.Identity.Name);

            if (carWashShops == null || !carWashShops.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShops-OwnerSide' " +
                    $"/ no shops in possession / '0' SHOPS FOUND ");
                return NotFound("You didn't create any CarWashShop yet..");
            }

            carWashShops = await _carWashShopRepository.QueryFilter(carWashShops, shopFilter);

            if (carWashShops == null || !carWashShops.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShops-OwnerSide' " +
                    $"/ no filtered results / '0' SHOPS FOUND ");
                return NotFound("There is no CarWashShop with specified filter parameters..");
            }

            var shopEntities = await _carWashShopRepository.Pagination(HttpContext, carWashShops, shopFilter.RecordsPerPage, shopFilter.Pagination);

            var shopsView = _mapper.Map<List<CarWashShopView>>(shopEntities);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' " +
                $"/ MethodName: 'GetShops-OwnerSide' / '{shopsView.Count}' SHOPS FOUND ");
            return shopsView;
        }

        //--3------------------------------ CREATE NEW SHOP WITH NEW SERVICES -------------------------------------- 

        [HttpPost("CreateNewShop", Name = "createNewShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Post([FromBody] CarWashShopCreation shopCreation)
        {
            string userName = User.Identity.Name;

            if (_dbContext.CarWashsShops.Any(x => x.Name == shopCreation.Name))
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'CreateNewShop-OwnerSide' " +
                    $"/ shop with a same name already exists / FAILED TO CREATE SHOP ");
                return BadRequest($"CarWashShop '{shopCreation.Name}' already exists..");
            }

            if (shopCreation.CarWashShopsOwners.Any())
                shopCreation.CarWashShopsOwners.ConvertAll(x => x.ToLower());

            if (shopCreation.Services == null || shopCreation.Services.Count == 0)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'CreateNewShop-OwnerSide' " +
                    $"/ no service was created with a shop / FAILED TO CREATE SHOP ");
                return BadRequest("Your shop needs to have at least one washing service..");
            }

            if (!shopCreation.CarWashShopsOwners.Contains(userName))
                shopCreation.CarWashShopsOwners.Add(userName);

            var carWashShopEntity = new CarWashShop();
            try
            {
                var OwnersIDs = await _carWashShopRepository.GetOwnerIDs(shopCreation);

                var services = _mapper.Map<List<Service>>(shopCreation.Services);
                _dbContext.Services.AddRange(services);
                await _dbContext.SaveChangesAsync();

                carWashShopEntity = _mapper.Map<CarWashShop>(shopCreation);
                carWashShopEntity.Owners = new List<CarWashShopsOwners>();

                OwnersIDs.ForEach(x => carWashShopEntity.Owners.Add(new CarWashShopsOwners { OwnerId = x }));
                services.ForEach(x => carWashShopEntity.CarWashShopsServices.Add(new CarWashShopsServices() { ServiceId = x.Id }));
                _dbContext.CarWashsShops.Add(carWashShopEntity);

                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'CreateNewShop-OwnerSide' " +
                    $"/ database exception {ex.Message} / FAILED TO CREATE SHOP ");
              return BadRequest("Your entries or entry formats are incorrect..");
            }

            var newCarWashShop = _mapper.Map<CarWashShopView>(carWashShopEntity);

            _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'CreateNewShop-OwnerSide' " +
                    $"/ NEW SHOP CREATED SUCCESSFULLY ");
            return newCarWashShop;
        }



        //--4----------------------------------------------- UPDATE SHOP'S GENERAL INFO -------------------------------------------------------------

        [HttpPut("UpdateShopInfo", Name = "updateShopInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Put(int shopID, [FromBody] CarWashShopUpdate shopUpdate)
        {
            string userName = User.Identity.Name;

            if (_dbContext.CarWashsShops.Any(x => x.Name == shopUpdate.Name))
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopInfo-OwnerSide' " +
                    $"/ shop with a same name already exists / FAILED TO UPDATE SHOP'S INFO ");
                return BadRequest($"CarWashShop '{shopUpdate.Name}' already exists..");
            }

            var carWashShopEntity = await _carWashShopRepository.GetShopByID(shopID, userName);

            if (carWashShopEntity == null)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopInfo-OwnerSide' " +
                    $"/ bad shop ID entry / FAILED TO UPDATE SHOP'S INFO ");
                return NotFound($"You don't have any CarWashShop with ID: '{shopID}'..");
            }

            carWashShopEntity = _mapper.Map(shopUpdate, carWashShopEntity);
            _dbContext.Entry(carWashShopEntity).State = EntityState.Modified;

            try { await _dbContext.SaveChangesAsync(); }

            catch(Exception ex)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopInfo-OwnerSide' " +
                    $"/ database exception {ex.Message} / FAILED TO UPDATE SHOP'S INFO ");
                return BadRequest("Your entries or are incorrect.."); 
            }
            
            var carShopView = _mapper.Map<CarWashShopView>(carWashShopEntity);

            _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'UpdateShopInfo-OwnerSide' " +
                    $"/ SHOP'S INFO SUCCESSFULLY UPDATED ");
            return carShopView;
        }



        //--5------------------------------------------------------ PATCH SHOP'S INFO  ---------------------------------------------------------------

        [HttpPatch("PatchShopInfo", Name = "patchShopInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Patch(int shopID, [FromBody] JsonPatchDocument<CarWashShopUpdate> shopUpdate)
        {
            string userName = User.Identity.Name;

            if (shopUpdate == null) 
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopInfo-OwnerSide' " +
                    $"/ property to patch is not specified / FAILED TO PATCH SHOP'S INFO ");
                return BadRequest("You didn't specify which info do you want to patch..");
            }

            var carShopEntity = await _carWashShopRepository.GetShopByID(shopID, userName);

            if (carShopEntity == null)
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopInfo-OwnerSide' " +
                    $"/ bad shop ID entry / FAILED TO PATCH SHOP'S INFO ");
                return NotFound($"You don't have any CarWashShop with ID: '{shopID}'..");
            }

            var carShopEntityPatch = _mapper.Map<CarWashShopUpdate>(carShopEntity);

            shopUpdate.ApplyTo(carShopEntityPatch, ModelState);

            var isValid = TryValidateModel(carShopEntityPatch);

            if (!isValid)
            {
                _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopInfo-OwnerSide' " +
                    $"/ patch model validation failed / FAILED TO PATCH SHOP'S INFO ");
                return BadRequest("Check your patch inputs..");
            }

            _mapper.Map(carShopEntityPatch, carShopEntity);
            await _dbContext.SaveChangesAsync();

            var carWashShopView = _mapper.Map<CarWashShopView>(carShopEntity);

            _logger.LogInformation($" / PATCH / UserName: '{userName.ToUpper()}' / MethodName: 'PatchShopInfo-OwnerSide' " +
                    $"/ SUCCESSFULLY TO PATCH SHOP'S INFO ");
            return carWashShopView;
        }



        //--6----------------------------------------------- REMOVE THE SHOP ------------------------------------------------- 

        [HttpDelete("RemoveShop", Name = "RemoveShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> Delete(int shopID, [FromBody] CarWashShopRemovalRequestCreation statementCreation)
         {
            string userName = User.Identity.Name;

            bool isRequestMadeAlready = await _carWashShopRepository.CheckIfRequestExist(shopID);
            if (isRequestMadeAlready)
            {
                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveShop-OwnerSide' " +
                    $"/ shop removal request already exists / NO REQUESTS MADE ");
                return BadRequest($"Removal request is already made for the CarWashShop with ID: '{shopID}'");
            }

            var carWashShop = await _carWashShopRepository.GetShopWithServicesByID(shopID, userName);

            if (carWashShop == null)
            {
                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveShop-OwnerSide' " +
                    $"/ bad shop ID entry / SHOP REMOVAL FAILED ");
                return BadRequest($"There is no CarWashShop with ID: '{shopID}' in your possession..");
            }

            string userId = await _carWashShopRepository.GetLoggedInOwnerID(carWashShop, userName);

            int amountOfOwners = carWashShop.Owners.Count;

            if (amountOfOwners > 1)
            {
                var ShopRemovalRequestList = await _carWashShopRepository.CreateShopRemovalRequests(carWashShop, userName, statementCreation);

                _dbContext.ShopRemovalRequests.AddRange(ShopRemovalRequestList);
                await _dbContext.SaveChangesAsync();
                
                string otherOwners = await _carWashShopRepository.ConcatenateCoOwnerNames(carWashShop, userName);

                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveShop-OwnerSide' " +
                    $"/ remove shop failed, multiple owners / SHOP REMOVAL REQUESTS ARE MADE ");
                return Ok($"Removal request has been made, because you are sharing ownership of the '{carWashShop.Name}' with {otherOwners}and now it awaits their approval..");
            }

            var carWashServices = carWashShop.CarWashShopsServices.Select(x => x.Service).ToList();

            _dbContext.Services.RemoveRange(carWashServices);
            _dbContext.CarWashsShops.Remove(carWashShop);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'RemoveShop-OwnerSide' " +
                    $"/ SHOP HAS BEEN SUCCESSFULLY REMOVED ");
            return Ok($"You have successfully removed '{carWashShop.Name}'..");
        }
    }
}
