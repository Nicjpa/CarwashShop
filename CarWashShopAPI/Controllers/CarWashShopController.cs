using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

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

        //--1------------------------------ GET ALL EXISTING SHOPS WITH FILTERS IN IN USER'S POSSESSION  ---------------------- 

        [HttpGet("GetAllShopsFilteredInYourPossessionOrByShopID", Name = "getAllShopsFilteredInYourPossessionOrByShopID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<List<CarWashShopView>>> Get([FromQuery] CarWashFilter shopFilter)
        {
            string userName = User.Identity.Name;
            var carShopEntities = _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .Include(x => x.Owner)
                .Where(x => x.Owner.UserName == userName).AsQueryable();

            var carWashShop = carShopEntities.Select(x => x.CarWashShop);
            if (carWashShop == null || carWashShop.Count() == 0)
                return NotFound("You didn't create any CarWashShop yet..");

            if (shopFilter.Id != null)
            {
                carWashShop = carWashShop.Where(x => x.Id == shopFilter.Id);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(shopFilter.Name))
                    carWashShop = carWashShop.Where(x => x.Name.Contains(shopFilter.Name));

                if (!string.IsNullOrWhiteSpace(shopFilter.AdvertisingDescription))
                    carWashShop = carWashShop.Where(x => x.AdvertisingDescription.Contains(shopFilter.AdvertisingDescription));

                if (shopFilter.MinimumAmountOfWashingUnits != null)
                    carWashShop = carWashShop.Where(x => x.AmountOfWashingUnits >= shopFilter.MinimumAmountOfWashingUnits);

                if (shopFilter.RequiredAndEarlierOpeningTime != null)
                    carWashShop = carWashShop.Where(x => x.OpeningTime <= shopFilter.RequiredAndEarlierOpeningTime);

                if (shopFilter.RequiredAndLaterClosingTime != null)
                    carWashShop = carWashShop.Where(x => x.ClosingTime >= shopFilter.RequiredAndLaterClosingTime);

                if (!string.IsNullOrWhiteSpace(shopFilter.ServiceNameOrDescription))
                    carWashShop = carWashShop
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(shopFilter.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(shopFilter.ServiceNameOrDescription)));
            }

            if (carWashShop == null || carWashShop.Count() == 0)
                return NotFound("There is no CarWashShop with specified filter parameters..");

            var shopsView = _mapper.Map<List<CarWashShopView>>(carWashShop);

            return Ok(shopsView);
        }

        //--3------------------------------ CREATE NEW SHOP WITH SERVICES BOUND TO THE EXISTING USER -------------------------------------- 

        [HttpPost("CreateNewShop", Name = "createNewShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> Post([FromBody] CarWashShopCreation shopCreation)
        {
            if (shopCreation.CarWashShopsOwners.Contains(null))
                return BadRequest("Each added shop owner must have name");
            else
                shopCreation.CarWashShopsOwners.ConvertAll(x => x.ToLower());

            if (shopCreation.Services == null || shopCreation.Services.Count == 0)
                return BadRequest("Your shop needs to have at least one washing service..");

            string userName = User.Identity.Name;

            if (!shopCreation.CarWashShopsOwners.Contains(userName))
                shopCreation.CarWashShopsOwners.Add(userName);

            var carWashShopEntity = new CarWashShop();

            try
            {
                var userIDs = await _dbContext.Users.Where(x => shopCreation.CarWashShopsOwners.Contains(x.UserName)).Select(x => x.Id).ToListAsync();

                var services = _mapper.Map<List<Service>>(shopCreation.Services);
                _dbContext.Services.AddRange(services);
                await _dbContext.SaveChangesAsync();

                carWashShopEntity = _mapper.Map<CarWashShop>(shopCreation);
                carWashShopEntity.Owners = new List<CarWashShopsOwners>();
                userIDs.ForEach(x => carWashShopEntity.Owners.Add(new CarWashShopsOwners { OwnerId = x }));
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

            return Ok($"You have successfully added NEW CarWashShop: '{carWashShopEntity.Name}'.");
        }



        //--4---------------------------------- UPDATE SHOP'S GENERAL INFO IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' ------------------------------- 

        [HttpPut("UpdateShopInfoByShopNameID", Name = "updateShopInfoByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Put(string UpdateShopInfoByShopNameOrId, [FromBody] CarWashShopUpdate shopUpdate)
        {
            bool isNotNumber = !int.TryParse(UpdateShopInfoByShopNameOrId, out int id) && UpdateShopInfoByShopNameOrId != "0";
            string type = isNotNumber ? "name" : "ID";

            string userName = User.Identity.Name;
            var ShopOwners = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Owner.UserName == userName && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == UpdateShopInfoByShopNameOrId.ToUpper()));

            if (ShopOwners == null)
                return NotFound($"You don't have any CarWashShop with {type} '{UpdateShopInfoByShopNameOrId}'..");

            var carShopEntity = ShopOwners.CarWashShop;
            carShopEntity = _mapper.Map(shopUpdate, carShopEntity);
            _dbContext.Entry(carShopEntity).State = EntityState.Modified;

            try { await _dbContext.SaveChangesAsync(); }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    return BadRequest($"Cannot change the name to '{shopUpdate.Name}', because a CarWashShop with that name already exists..");

                return BadRequest("Your entries or are incorrect..");
            }

            var carShopView = _mapper.Map<CarWashShopUpdate>(carShopEntity);

            return Ok(carShopView);
        }



        //--5---------------------------------- PATCH CERTAIN SHOP'S INFO IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' ------------------------------- 

        [HttpPatch("PatchShopInfoByShopNameID", Name = "patchShopInfoByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Patch(string PatchShopInfoByShopNameOrId, [FromBody] JsonPatchDocument<CarWashShopUpdate> shopUpdate)
        {
            if (shopUpdate == null) { return BadRequest("You didn't specify which info do you want to patch.."); }

            bool isNotNumber = !int.TryParse(PatchShopInfoByShopNameOrId, out int id) && PatchShopInfoByShopNameOrId != "0";
            string type = isNotNumber ? "name" : "ID";

            string userName = User.Identity.Name;
            var ShopOwners = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Owner.UserName == userName && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == PatchShopInfoByShopNameOrId.ToUpper()));

            if (ShopOwners == null)
                return NotFound($"You don't have any CarWashShop with {type} '{PatchShopInfoByShopNameOrId}'..");
            var carShopEntity = ShopOwners.CarWashShop;

            var carShopEntityPatch = _mapper.Map<CarWashShopUpdate>(carShopEntity);

            shopUpdate.ApplyTo(carShopEntityPatch, ModelState);

            var isValid = TryValidateModel(carShopEntityPatch);
            if (!isValid) { return BadRequest("Check your patch inputs.."); }

            _mapper.Map(carShopEntityPatch, carShopEntity);
            await _dbContext.SaveChangesAsync();

            return Ok(carShopEntityPatch);
        }



        //--6----------------------------------------------- REMOVE CAR WASH SHOP ------------------------------------------------- 

        [HttpDelete("DeleteCarWashShop", Name = "deleteCarWashShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> Delete([FromBody] CarWashShopRemovalRequestCreation cwShopRemovalRequest)
        {
            string userName = User.Identity.Name;

            bool isRequestMadeAlready = _dbContext.ShopRemovalRequests.Include(x => x.CarWashShop).Any(x => x.CarWashShop.Name.ToUpper() == cwShopRemovalRequest.CarWashShopName.ToUpper());
            if (isRequestMadeAlready)
                return BadRequest($"Removal request is already made for the '{cwShopRemovalRequest.CarWashShopName}'");

            var carWashShop = await _dbContext.CarWashsShops
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Name == cwShopRemovalRequest.CarWashShopName && x.Owners.Select(x => x.Owner.UserName).Contains(userName));

            if (carWashShop == null)
                return BadRequest($"There is no CarWashShop '{carWashShop.Name}' in your possession..");

            string userId = carWashShop.Owners.Where(x => x.Owner.UserName == userName).Select(x => x.Owner.Id).FirstOrDefault();

            int amountOfOwners = carWashShop.Owners.Count();

            if (amountOfOwners > 1)
            {
                var OwnersIDs = carWashShop.Owners.Where(x => x.Owner.UserName != userName).Select(x => x.Owner.Id).ToList();
                var cwShopRemovalRequestList = new List<CarWashShopRemovalRequest>();

                OwnersIDs.ForEach(x => cwShopRemovalRequestList.Add(new CarWashShopRemovalRequest()
                {
                    OwnerId = x,
                    CarWashShopId = carWashShop.Id,
                    RequestStatement = cwShopRemovalRequest.RequestStatement
                }));

                _dbContext.ShopRemovalRequests.AddRange(cwShopRemovalRequestList);
                await _dbContext.SaveChangesAsync();

                string otherOwners = string.Empty;
                carWashShop.Owners.ForEach(x => otherOwners += $"{x.Owner.UserName}, ");
                otherOwners = otherOwners.Replace($"{userName}, ", "");

                return Ok($"Removal request has been made, because you are sharing ownership of the '{cwShopRemovalRequest.CarWashShopName}' with {otherOwners}and now it awaits their approval..");
            }

            var carWashServices = new List<Service>();
            carWashServices = carWashShop.CarWashShopsServices.Select(x => x.Service).ToList();

            _dbContext.Services.RemoveRange(carWashServices);
            _dbContext.CarWashsShops.Remove(carWashShop);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully removed '{carWashShop.Name}'..");
        }
    }
}
