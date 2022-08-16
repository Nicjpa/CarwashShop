using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Controllers
{
    [Route("api/Owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public OwnerController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        //--1------------------------------ GET LIST OF OWNERS FOR EACH SHOP IN USER'S POSSESSION WITH FILTERS  ----------------------  

        [HttpGet("GetAllOwnersFilteredOrByShopNameID", Name = "getAllOwnersFilteredOrByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<List<OwnersViewPerShop>>> GetOwners([FromQuery]ListOfOwnersPerShopFilters filters)
        {
            string userName = User.Identity.Name;

            var carWashShopEntities = _dbContext.CarWashsShops
                .Include(x => x.Owners).ThenInclude(x => x.Owner)
                .Where(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .AsQueryable();

            if (filters.CarWashShopId != null)
            {
                carWashShopEntities = carWashShopEntities.Where(x => x.Id == filters.CarWashShopId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    carWashShopEntities = carWashShopEntities.Where(x => x.Name == filters.CarWashShopName);
            }


            if (carWashShopEntities == null || carWashShopEntities.Count() == 0)
                return Ok("No car wash shop found..");

            var ownersView = _mapper.Map<List<OwnersViewPerShop>>(carWashShopEntities);

            return Ok(ownersView);
        }



        //--3----------------------------------------------- GET ALL DISBAND REQUESTS WITH FILTERS ------------------------------------------------- 

        [HttpGet("GetAllDisbandRequestsOrByShopNameID", Name = "getAllDisbandRequestsOrByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<List<OwnerDisbandRequestView>>> GetDisbandRequests([FromQuery] OwnerRequestsFilters filters)
        {
            string userName = User.Identity.Name;

            var allDisbandRequestsEntities = _dbContext.OwnerRemovalRequests
                .Include(x => x.Requester)
                .Include(x => x.CarWashShop)
                .Include(x => x.OwnerToBeRemoved)
                .Where(x => x.OwnerToBeRemoved.UserName == userName)
                .AsQueryable();

            if (filters.CarWashShopId != null)
            {
                allDisbandRequestsEntities = allDisbandRequestsEntities.Where(x => x.CarWashShopId == filters.CarWashShopId);
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    allDisbandRequestsEntities = allDisbandRequestsEntities.Where(x => x.CarWashShop.Name == filters.CarWashShopName);

                if (filters.NotApproved)
                    allDisbandRequestsEntities = allDisbandRequestsEntities.Where(x => !x.IsApproved);
            }

            if (allDisbandRequestsEntities == null || allDisbandRequestsEntities.Count() == 0)
                return NotFound("No disband request found..");

            var allDisbandRequestsView = _mapper.Map<List<OwnerDisbandRequestView>>(allDisbandRequestsEntities);

            return Ok(allDisbandRequestsView);
        }



        //--5----------------------------------------------- GET ALL SHOP REMOVAL REQUESTS WITH FILTERS------------------------------------------------- 

        [HttpGet("GetAllShopRemovalRequestsOrByShopNameID", Name = "getAllShopRemovalRequestsOrByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<List<OwnerShopRemovalRequestView>>> GetShopRemovalRequests([FromQuery] OwnerRequestsFilters filters)
        {
            string userName = User.Identity.Name;

            var allShopRemovalRequestsEntities =  _dbContext.ShopRemovalRequests
                .Include(x => x.Owner)
                .Include(x => x.CarWashShop)
                .Where(x => x.Owner.UserName == userName)
                .AsQueryable();

            if (filters.CarWashShopId != null)
            {
                allShopRemovalRequestsEntities = allShopRemovalRequestsEntities.Where(x => x.CarWashShopId == filters.CarWashShopId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filters.CarWashShopName))
                    allShopRemovalRequestsEntities = allShopRemovalRequestsEntities.Where(x => x.CarWashShop.Name == filters.CarWashShopName);

                if (filters.NotApproved)
                    allShopRemovalRequestsEntities = allShopRemovalRequestsEntities.Where(x => !x.IsApproved);
            }


            if (allShopRemovalRequestsEntities == null || allShopRemovalRequestsEntities.Count() == 0)
                return NotFound("There is no shop removal requests for you..");

            var allShopRemovalRequestsView = _mapper.Map<List<OwnerShopRemovalRequestView>>(allShopRemovalRequestsEntities);

            return Ok(allShopRemovalRequestsView);
        }



        //--7---------------------------------- ADD NEW OWNERS TO CAR WASH SHOP IN USER'S POSSESSION BY 'ShopName' ------------------------------ 

        [HttpPost("AddOwnerToTheCarWashShopByShopNameID", Name = "addOwnerToTheCarWashShopByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult<CarWashShopView>> Post([FromBody] CarWashShopOwnerAdd newOwners)
        {
            string userName = User.Identity.Name;

            var carWashShop = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Name.ToUpper() == newOwners.CarWashShopName.ToUpper());

            if (carWashShop == null)
                return NotFound($"CarWashShop with name '{newOwners.CarWashShopName}' doesn't exist..");

            var currentOwnerList = carWashShop.Owners.Select(x => x.Owner.UserName).ToList();
            if (!currentOwnerList.Contains(userName))
                return BadRequest($"You don't have access to '{newOwners.CarWashShopName}'..");

            var PotentialOwnersUserNames = newOwners.OwnerUserName.ConvertAll(x => x.ToLower());

            var PotentialOwnersIDs = await _dbContext.Users
                .Where(x => PotentialOwnersUserNames
                .Contains(x.UserName))
                .Select(x => x.Id)
                .ToListAsync();

            var CurrentOwnerUserIds = new List<string>();
            carWashShop.Owners.ForEach(x => CurrentOwnerUserIds.Add(x.Owner.Id));

            var approvedOwnersIDs = await _dbContext.UserClaims
                .Where(x => PotentialOwnersIDs
                .Contains(x.UserId) && x.ClaimValue == "Owner")
                .Select(x => x.UserId)
                .ToListAsync();

            var legitNewOwners = new List<CarWashShopsOwners>();
            foreach (string id in approvedOwnersIDs)
            {
                if (!CurrentOwnerUserIds.Contains(id))
                {
                    legitNewOwners.Add(new CarWashShopsOwners() { CarWashShopId = carWashShop.Id, OwnerId = id });
                }
            }

            _dbContext.CarWashShopsOwners.AddRange(legitNewOwners);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully added {approvedOwnersIDs.Count} more owners..");
        }



        //--8---------------------------------- REQUEST OWNER REMOVAL FROM THE CAR WASH SHOP ------------------------------ 

        [HttpPost("RequestOwnerRemoval", Name = "requestOwnerRemoval")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> PostOwnerRemovalRequest([FromBody] OwnerRemovalRequestCreation ownerRemovalRequest)
        {
            string userName = User.Identity.Name;
            string ownerNameToRemove = ownerRemovalRequest.OwnerName.ToLower();

            if (userName == ownerRemovalRequest.OwnerName.ToLower())
                return BadRequest("You cannot remove yourself..");

            bool isRequestMadeAlready = _dbContext.OwnerRemovalRequests
                .Include(x => x.OwnerToBeRemoved)
                .Include(x => x.CarWashShop)
                .Any(x => x.CarWashShop.Name.ToUpper() == ownerRemovalRequest.CarWashShopName.ToUpper() && x.OwnerToBeRemoved.UserName == ownerNameToRemove);

            if (isRequestMadeAlready)
                return BadRequest($"The removal request for the owner '{ownerNameToRemove}' already exists..");

            var carWashShop = await _dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Name == ownerRemovalRequest.CarWashShopName
                                    && x.Owners.Select(x => x.Owner.UserName).Contains(ownerNameToRemove));

            if (carWashShop == null)
                return BadRequest($"Owner name '{ownerNameToRemove}' doesn't match with the CarWashShopName '{ownerRemovalRequest.CarWashShopName}'..");
            else
            {
                bool isUserOwner = carWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName);
                if (!isUserOwner)
                    return BadRequest($"You don't have access to the CarWashShop '{carWashShop.Name}'..");
            }

            var removalRequest = new OwnerRemovalRequest()
            {
                RequestStatement = ownerRemovalRequest.RequestStatement,
                CarWashShopId = carWashShop.Id,
                RequesterId = carWashShop.Owners.Where(x => x.Owner.UserName == userName).Select(x => x.OwnerId).FirstOrDefault(),
                OwnerToBeRemovedId = carWashShop.Owners.Where(x => x.Owner.UserName == ownerNameToRemove).Select(x => x.OwnerId).FirstOrDefault()
            };

            _dbContext.OwnerRemovalRequests.Add(removalRequest);
            await _dbContext.SaveChangesAsync();

            return Ok($"Request to remove '{ownerNameToRemove}' as the owner of the '{ownerRemovalRequest.CarWashShopName}' has been made by '{userName}', and now it awaits approval from the owner.");
        }



        //--9----------------------------------------------- APPROVE TO BE DISBANDED AS THE OWNER OF THE SHOP ------------------------------------------------- 

        [HttpPut("ApproveDisbandFromTheShopByShopNameID", Name = "approveDisbandFromTheShopByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> ApproveDisbandFromShop(string ApproveDisbandFromTheShop)
        {
            string userName = User.Identity.Name;
            bool isNotNumber = !int.TryParse(ApproveDisbandFromTheShop, out int id) && ApproveDisbandFromTheShop != "0";
            string type = isNotNumber ? "name" : "ID";

            var ownerRemovalRequest = await _dbContext.OwnerRemovalRequests
                .Include(x => x.CarWashShop)
                .Include(x => x.OwnerToBeRemoved)
                .FirstOrDefaultAsync(x => x.OwnerToBeRemoved.UserName == userName && (x.CarWashShop.Id == id || x.CarWashShop.Name.ToUpper() == ApproveDisbandFromTheShop.ToUpper()) && !x.IsApproved);

            if (ownerRemovalRequest == null)
                return NotFound($"There is no request for you under the CarWashShop {type} '{ApproveDisbandFromTheShop}'");

            ownerRemovalRequest.IsApproved = true;

            _dbContext.Entry(ownerRemovalRequest).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok($"You have approved to be disbanded from the CarWashShop '{ownerRemovalRequest.CarWashShop.Name}'!");
        }



        //--10----------------------------------------------- APPROVE CAR WASH SHOP REMOVAL BY 'ShopName' OR 'ShopID' ------------------------------------------------- 

        [HttpPut("ApproveShopRemovalByShopNameID", Name = "ApproveShopRemovalByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> ApproveShopRemoval(string ApproveShopRemovalByShopNameOrID)
        {
            string userName = User.Identity.Name;
            bool isNotNumber = !int.TryParse(ApproveShopRemovalByShopNameOrID, out int id) && ApproveShopRemovalByShopNameOrID != "0";
            string type = isNotNumber ? "name" : "ID";

            var shopRemovalRequest = await _dbContext.ShopRemovalRequests
            .Include(x => x.Owner)
            .Include(x => x.CarWashShop)
            .Where(x => (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == ApproveShopRemovalByShopNameOrID.ToUpper()) && !x.IsApproved && x.Owner.UserName == userName)
            .ToListAsync();

            if (shopRemovalRequest.Count == 0)
                return NotFound($"there is no removal requests for the car wash shop with {type} '{ApproveShopRemovalByShopNameOrID}'..");

            var requestToApprove = shopRemovalRequest.FirstOrDefault(x => x.Owner.UserName == userName);
            requestToApprove.IsApproved = true;

            _dbContext.Entry(requestToApprove).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok($"You have approved removal of the CarWashShop '{requestToApprove.CarWashShop.Name}'!");
        }



        //--11----------------------------------------------- CANCEL CAR WASH SHOP REMOVAL REQUEST BY 'ShopName' OR 'ShopID' ------------------------------------------------- 

        [HttpDelete("CancelShopRemovalByShopNameID", Name = "cancelShopRemovalByShopNameID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owner")]
        public async Task<ActionResult> CancelShopRemovalRequest(string CancelShopRemovalByShopNameOrID)
        {
            string userName = User.Identity.Name;
            int.TryParse(CancelShopRemovalByShopNameOrID, out int id);

            var cwShopRemovalRequests = await _dbContext.ShopRemovalRequests
                .Include(x => x.CarWashShop)
                .ThenInclude(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .Where(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == CancelShopRemovalByShopNameOrID.ToUpper())
                .ToListAsync();

            if (cwShopRemovalRequests == null)
                return NotFound($"Removal request for this CarWashShop doesn't exist, please check your input if it's correct ..");

            bool isOwner = cwShopRemovalRequests.Any(x => x.CarWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName));
            if (!isOwner)
                return BadRequest("You are not authorized to cancel this request..");

            _dbContext.ShopRemovalRequests.RemoveRange(cwShopRemovalRequests);
            await _dbContext.SaveChangesAsync();

            return Ok($"Removal request of the '{cwShopRemovalRequests.Select(x => x.CarWashShop.Name).FirstOrDefault()}' is successfully canceled");
        }



       



        
    }
}
