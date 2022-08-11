using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
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
using System.Text;

namespace CarWashShopAPI.Controllers
{
    [Route("api/CarWashShop")]
    [ApiController]
    public class CarWashShopController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public CarWashShopController(CarWashDbContext dbContext, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        //--1------------------------------ GET ALL EXISTING SHOPS IN IN USER'S POSSESSION  ----------------------

        [HttpGet("ListShopsInYourPossession", Name = "listShopsInYourPossession")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> GetAllShopsInPossession()
        {
            string ownerId = User.Identity.Name.ToUpper();
            var carShopEntities = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .Where(x => x.OwnerId.ToUpper() == ownerId).ToListAsync();

            var carWashShop = carShopEntities.Select(x => x.CarWashShop);
            if (carWashShop == null || carWashShop.Count() == 0)
                return Ok("You didn't create any CarWashShop yet..");


            var shopsView = _mapper.Map<List<CarWashShopView>>(carWashShop);
            var servicesView = _mapper.Map<List<CarWashShopView>>(carShopEntities);

            for (int i = 0; i < shopsView.Count; i++)
            {
                shopsView[i].Services = servicesView[i].Services;
            }
            return Ok(shopsView);
        }



        //--2------------------------------ GET EXISTING SHOP BY 'ShopName' OR 'ShopID' IN USER'S POSSESSION  ----------------------

        [HttpGet("GetYourShopByNameOrId", Name = "getYourShopByNameOrId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CarWashShopView>> GetYourShopByNameOrId(string GetYourShopByNameOrId)
        {
            bool isNotNumber = !int.TryParse(GetYourShopByNameOrId, out int id) && GetYourShopByNameOrId != "0";
            string type = isNotNumber ? "name" : "ID";

            string ownerId = User.Identity.Name.ToUpper();
            var carShopEntities = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .FirstOrDefaultAsync(x => x.OwnerId.ToUpper() == ownerId && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == GetYourShopByNameOrId.ToUpper()));

            if (carShopEntities == null)
                return NotFound($"You don't have any CarWashShop with {type} '{GetYourShopByNameOrId}'..");

            var carWashShop = carShopEntities.CarWashShop;
            var shopsView = _mapper.Map<CarWashShopView>(carWashShop);
            var servicesView = _mapper.Map<CarWashShopView>(carShopEntities);
            shopsView.Services = servicesView.Services;

            return Ok(shopsView);
        }



        //--3------------------------------ GET LIST OF OWNERS FOR EACH SHOP IN USER'S POSSESSION  ----------------------

        [HttpGet("ListOwnersForEachShopInPossession", Name = "listOwnersForEachShopInPossession")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<OwnersViewPerShop>>> ListAllOwnersPerShop()
        {
            string ownerId = User.Identity.Name.ToUpper();
            var carShopsOwnersEntities = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .Where(x => x.OwnerId.ToUpper() == ownerId).ToListAsync();

            var carWashShop = carShopsOwnersEntities.Select(x => x.CarWashShop);
            if (carWashShop == null || carWashShop.Count() == 0)
                return Ok("You don't have any CarWashShop created..");

            var ownersView = _mapper.Map<List<OwnersViewPerShop>>(carWashShop);

            return Ok(ownersView);
        }




        //PROVERI NA OVOM KONTROLERU
        //--4------------------------------ GET LIST OF OWNERS FOR THE SHOP IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' ----------------------

        [HttpGet("ListOwnersByShopNameOrShopId", Name = "listOwnersByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<OwnersViewPerShop>> ListOwnersByShopNameOrId(string ListOwnersByShopNameOrShopId)
        {
            bool isNotNumber = !int.TryParse(ListOwnersByShopNameOrShopId, out int id) && ListOwnersByShopNameOrShopId != "0";
            string type = isNotNumber ? "name" : "ID";
            string ownerId = User.Identity.Name.ToUpper();
            var carWashShopEntity = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                //.FirstOrDefaultAsync(x => x.Owners.Select(x => x.OwnerId.ToUpper()).Contains(ownerId) && (x.Id == id || x.Name.ToUpper() == ListOwnersByShopNameOrShopId.ToUpper()));
            .FirstOrDefaultAsync(x => x.Owners.Select(x => x.OwnerId.GetUserName(ownerId)).Contains(ownerId) && (x.Id == id || x.Name.ToUpper() == ListOwnersByShopNameOrShopId.ToUpper()));



            if (carWashShopEntity == null)
                return NotFound($"You don't have any CarWashShop with {type} '{ListOwnersByShopNameOrShopId}'..");

            var ownerView = _mapper.Map<OwnersViewPerShop>(carWashShopEntity);

            return Ok(ownerView);
        }



        //--5------------------------------ CREATE NEW SHOP BOUND TO THE EXISTING USER --------------------------------------

        [HttpPost("CreateYourShop", Name = "createYourShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] CarWashShopCreation shopCreation)
        {
            if (shopCreation.CarWashShopsOwners.Contains(null))
                return BadRequest("Each added shop owner must have name");
            if (shopCreation.Services == null || shopCreation.Services.Count == 0)
                return BadRequest("Your shop needs to have at least one washing service..");

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



        //--6---------------------------------- UPDATE SHOP'S GENERAL INFO IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' -------------------------------

        [HttpPut("UpdateShopInfoByShopNameOrId", Name = "updateShopInfoByShopNameOrId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CarWashShopView>> Put(string UpdateShopInfoByShopNameOrId, [FromBody] CarWashShopUpdate shopUpdate)
        {
            bool isNotNumber = !int.TryParse(UpdateShopInfoByShopNameOrId, out int id) && UpdateShopInfoByShopNameOrId != "0";
            string type = isNotNumber ? "name" : "ID";

            string ownerId = User.Identity.Name.ToUpper();
            var ShopOwners = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .FirstOrDefaultAsync(x => x.OwnerId.ToUpper() == ownerId && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == UpdateShopInfoByShopNameOrId.ToUpper()));

            if (ShopOwners == null)
                return NotFound($"You don't have any CarWashShop with {type} '{UpdateShopInfoByShopNameOrId}'..");

            var carShopEntity = ShopOwners.CarWashShop;
            carShopEntity = _mapper.Map(shopUpdate, carShopEntity);
            _dbContext.Entry(carShopEntity).State = EntityState.Modified;

            try { await _dbContext.SaveChangesAsync(); }
            catch(Exception ex) 
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                    return BadRequest($"Cannot change the name to '{shopUpdate.Name}', because a CarWashShop with that name already exists..");

                return BadRequest("Your entries or entry formats are incorrect..");
            }

            var carShopView = _mapper.Map<CarWashShopView>(carShopEntity);

            return Ok(carShopView);
        }



        //--7---------------------------------- PATCH CERTAIN SHOP'S INFO IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' -------------------------------

        [HttpPatch("PatchShopInfoByShopNameOrId", Name = "patchShopInfoByShopNameOrId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CarWashShopView>> Patch(string PatchShopInfoByShopNameOrId, [FromBody] JsonPatchDocument<CarWashShopUpdate>  shopUpdate)
        {
            if (shopUpdate == null) { return BadRequest("You didn't specify which info do you want to patch.."); }

            bool isNotNumber = !int.TryParse(PatchShopInfoByShopNameOrId, out int id) && PatchShopInfoByShopNameOrId != "0";
            string type = isNotNumber ? "name" : "ID";

            string ownerId = User.Identity.Name.ToUpper();
            var ShopOwners = await _dbContext.CarWashShopsOwners
                .Include(x => x.CarWashShop)
                .FirstOrDefaultAsync(x => x.OwnerId.ToUpper() == ownerId && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == PatchShopInfoByShopNameOrId.ToUpper()));

            var carShopEntity = ShopOwners.CarWashShop;
            if (carShopEntity == null)
                return NotFound($"You don't have any CarWashShop with {type} '{PatchShopInfoByShopNameOrId}'..");

            var carShopEntityPatch = _mapper.Map<CarWashShopUpdate>(carShopEntity);

            shopUpdate.ApplyTo(carShopEntityPatch, ModelState);

            var isValid = TryValidateModel(carShopEntityPatch);
            if (!isValid) { return BadRequest("Check your patch inputs.."); }

            _mapper.Map(carShopEntityPatch, carShopEntity);
            await _dbContext.SaveChangesAsync();

            return Ok(carShopEntityPatch);
        }



        //--8---------------------------------- ADD NEW OWNERS TO CAR WASH SHOP IN USER'S POSSESSION BY 'ShopName' ----------------------------------------------------

        [HttpPost("AddOwnerToTheCarWashShopByShopNameOrShopId", Name = "addOwnerToTheCarWashShopByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CarWashShopView>> Post([FromBody]CarWashShopOwnerAdd newOwners)
        {
            string ownerId = User.Identity.Name.ToUpper();
            //bool hasRoleOwner = User.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Owner");
            
            var carWashShop = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == newOwners.CarWashShopName.ToUpper());
            if (carWashShop == null)
                return NotFound($"CarWashShop with name '{newOwners.CarWashShopName}' doesn't exist..");

            var currentOwnerList = carWashShop.Owners.Select(x => x.OwnerId.ToUpper()).ToList();
            if (!currentOwnerList.Contains(ownerId))
                return BadRequest($"You don't have access to '{newOwners.CarWashShopName}'..");

            var userClaims = await _dbContext.UserClaims.Where(x => newOwners.OwnerIDs.Contains(x.UserId) && x.ClaimValue == "Owner").ToListAsync();
            var usersWithOwnerClaimRole = userClaims.Select(x => x.UserId);

            string statusReport = string.Empty;
            StringBuilder sb = new StringBuilder();

            var legitOwnersList = new List<CarWashShopsOwners>();
            foreach(string owner in newOwners.OwnerIDs)
            {
                
                if (currentOwnerList.Contains(owner.ToUpper()))
                {
                    sb.AppendLine($" * '{owner}' is already shop owner *");
                }
                else if (usersWithOwnerClaimRole.Contains(owner.ToUpper()))
                {
                    legitOwnersList.Add(new CarWashShopsOwners() { CarWashShopId = carWashShop.Id, OwnerId = owner });
                    sb.AppendLine($" * '{owner}' is successfully added among other shop owners *");
                }
                else
                {
                    sb.AppendLine($" * '{owner}' failed to be added, because it's not registered as 'Owner' *");
                }
            }
            _dbContext.CarWashShopsOwners.AddRange(legitOwnersList);
            await _dbContext.SaveChangesAsync();

            statusReport = sb.ToString();

            return Ok(statusReport);
        }



        //--9---------------------------------------- ADD NEW SERVICE TO EXISTING SHOP IN USER'S POSSESSION ---------------------------------------------

        [HttpPost("AddNewServiceToShopByShopNameOrShopId", Name = "addNewServiceToShopByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(string AddNewServiceToShopByShopNameOrShopId, [FromBody] ServiceCreationView newServiceCreation)
        {
            string user = User.Identity.Name.ToUpper();
            var possibleUserNames = new List<string>();

            bool isNotNumber = !int.TryParse(AddNewServiceToShopByShopNameOrShopId, out int id) && AddNewServiceToShopByShopNameOrShopId != "0";
            string type = isNotNumber ? "a name" : "ID";

            var shopServiceEntity = await _dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop).ThenInclude(x => x.Owners)
                .FirstOrDefaultAsync(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == AddNewServiceToShopByShopNameOrShopId.ToUpper());

            if (shopServiceEntity == null)
                return NotFound($"There is no CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");

            shopServiceEntity.CarWashShop.Owners.ForEach(x => possibleUserNames.Add(x.OwnerId.ToUpper()));
            if (!possibleUserNames.Contains(user))
                return BadRequest($"You don't have access to the CarWashShop with {type} '{AddNewServiceToShopByShopNameOrShopId}'..");

            var newServiceEntity = _mapper.Map<Service>(newServiceCreation);
            shopServiceEntity.Service = newServiceEntity;

            _dbContext.CarWashShopsServices.Add(shopServiceEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully added a NEW service '{newServiceEntity.Name}' at your '{shopServiceEntity.CarWashShop.Name}' CarWashShop.");
        }
    }
}
