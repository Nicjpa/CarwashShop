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

        public OwnerCarWashShopCRUDController(CarWashDbContext dbContext, IMapper mapper, ICarWashRepository carWashShopRepository
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _carWashShopRepository = carWashShopRepository;
        }



        //--1------------------------------ GET ALL EXISTING SHOPS IN POSSESSION ---------------------- 

        [HttpGet("GetAllShops-OwnersSide", Name = "getAllShops-OwnersSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<CarWashShopView>>> Get([FromQuery] CarWashFilter shopFilter)
        {
            var carWashShops = await _carWashShopRepository.GetAllShopsInPossesion(User.Identity.Name);

            if (carWashShops == null || !carWashShops.Any())
                return NotFound("You didn't create any CarWashShop yet..");

            carWashShops = await _carWashShopRepository.QueryFilter(carWashShops, shopFilter);

            if (carWashShops == null || !carWashShops.Any())
                return NotFound("There is no CarWashShop with specified filter parameters..");

            var shopEntities = await _carWashShopRepository.Pagination(HttpContext, carWashShops, shopFilter.RecordsPerPage, shopFilter.Pagination);

            var shopsView = _mapper.Map<List<CarWashShopView>>(shopEntities);

            return Ok(shopsView);
        }

        //--3------------------------------ CREATE NEW SHOP WITH NEW SERVICES -------------------------------------- 

        [HttpPost("CreateNewShop", Name = "createNewShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult> Post([FromBody] CarWashShopCreation shopCreation)
        {
            string userName = User.Identity.Name;

            if (shopCreation.CarWashShopsOwners.Contains(null))
                return BadRequest("Each added shop owner must have name");
            else
                shopCreation.CarWashShopsOwners.ConvertAll(x => x.ToLower());

            if (shopCreation.Services == null || shopCreation.Services.Count == 0)
                return BadRequest("Your shop needs to have at least one washing service..");

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
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    return BadRequest($"CarWashShop '{carWashShopEntity.Name}' already exists..");

                return BadRequest("Your entries or entry formats are incorrect..");
            }

            var newCarWashShop = _mapper.Map<CarWashShopView>(carWashShopEntity);

            return new CreatedAtRouteResult("getAllShops-OwnersSide", new { newCarWashShop.Id }, newCarWashShop);
        }



        //--4----------------------------------------------- UPDATE SHOP'S GENERAL INFO -------------------------------------------------------------

        [HttpPut("UpdateShopInfo", Name = "updateShopInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Put(int shopID, [FromBody] CarWashShopUpdate shopUpdate)
        {
            string userName = User.Identity.Name;
            var carWashShopEntity = await _carWashShopRepository.GetShopByID(shopID, userName);

            if (carWashShopEntity == null)
                return NotFound($"You don't have any CarWashShop with ID: '{shopID}'..");

            carWashShopEntity = _mapper.Map(shopUpdate, carWashShopEntity);
            _dbContext.Entry(carWashShopEntity).State = EntityState.Modified;

            try { await _dbContext.SaveChangesAsync(); }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    return BadRequest($"Cannot change the name to '{shopUpdate.Name}', because a CarWashShop with that name already exists..");

                return BadRequest("Your entries or are incorrect..");
            }

            var carShopView = _mapper.Map<CarWashShopView>(carWashShopEntity);

            return Ok(carShopView);
        }



        //--5------------------------------------------------------ PATCH SHOP'S INFO  ---------------------------------------------------------------

        [HttpPatch("PatchShopInfo", Name = "patchShopInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Patch(int shopID, [FromBody] JsonPatchDocument<CarWashShopUpdate> shopUpdate)
        {
            if (shopUpdate == null) { return BadRequest("You didn't specify which info do you want to patch.."); }

            string userName = User.Identity.Name;
            var carShopEntity = await _carWashShopRepository.GetShopByID(shopID, userName);

            if (carShopEntity == null)
                return NotFound($"You don't have any CarWashShop with ID: '{shopID}'..");

            var carShopEntityPatch = _mapper.Map<CarWashShopUpdate>(carShopEntity);

            shopUpdate.ApplyTo(carShopEntityPatch, ModelState);

            var isValid = TryValidateModel(carShopEntityPatch);

            if (!isValid) 
                return BadRequest("Check your patch inputs.."); 

            _mapper.Map(carShopEntityPatch, carShopEntity);
            await _dbContext.SaveChangesAsync();

            var carWashShopView = _mapper.Map<CarWashShopView>(carShopEntity);

            return Ok(carWashShopView);
        }



        //--6----------------------------------------------- REMOVE THEB SHOP ------------------------------------------------- 

        [HttpDelete("RemoveShop", Name = "RemoveShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult> Delete(int shopID, [FromBody] CarWashShopRemovalRequestCreation statementCreation)
        {
            string userName = User.Identity.Name;

            bool isRequestMadeAlready = _dbContext.ShopRemovalRequests.Include(x => x.CarWashShop).Any(x => x.CarWashShop.Id == shopID);
            if (isRequestMadeAlready)
                return BadRequest($"Removal request is already made for the CarWashShop with ID: '{shopID}'");

            var carWashShop = await _carWashShopRepository.GetShopWithServicesByID(shopID, userName);

            if (carWashShop == null)
                return BadRequest($"There is no CarWashShop with ID: '{shopID}' in your possession..");

            string userId = await _carWashShopRepository.GetLoggedInOwnerID(carWashShop, userName);

            int amountOfOwners = carWashShop.Owners.Count;

            if (amountOfOwners > 1)
            {
                var ShopRemovalRequestList = await _carWashShopRepository.CreateShopRemovalRequests(carWashShop, userName, statementCreation);

                _dbContext.ShopRemovalRequests.AddRange(ShopRemovalRequestList);
                await _dbContext.SaveChangesAsync();
                
                string otherOwners = await _carWashShopRepository.ConcatenateCoOwnerNames(carWashShop, userName);

                return Ok($"Removal request has been made, because you are sharing ownership of the '{carWashShop.Name}' with {otherOwners}and now it awaits their approval..");
            }

            var carWashServices = carWashShop.CarWashShopsServices.Select(x => x.Service).ToList();

            _dbContext.Services.RemoveRange(carWashServices);
            _dbContext.CarWashsShops.Remove(carWashShop);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully removed '{carWashShop.Name}'..");
        }
    }
}
