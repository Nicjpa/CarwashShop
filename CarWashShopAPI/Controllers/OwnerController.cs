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



        //--1------------------------------ GET LIST OF OWNERS FOR EACH SHOP IN USER'S POSSESSION  ----------------------  

        [HttpGet("ListOwnersForEachShopInPossession", Name = "listOwnersForEachShopInPossession")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<OwnersViewPerShop>>> ListAllOwnersPerShop()
        {
            string userName = User.Identity.Name;

            var carWashShopEntities = await _dbContext.CarWashsShops
                .Include(x => x.Owners).ThenInclude(x => x.Owner)
                .Where(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName))
                .ToListAsync();

            if (carWashShopEntities == null || carWashShopEntities.Count() == 0)
                return Ok("You don't have any CarWashShop created..");

            var ownersView = _mapper.Map<List<OwnersViewPerShop>>(carWashShopEntities);

            return Ok(ownersView);
        }



        //--2------------------------------ GET LIST OF OWNERS FOR THE SHOP IN USER'S POSSESSION BY 'ShopName' OR 'ShopID' ----------------------  

        [HttpGet("ListOwnersByShopNameOrShopId", Name = "listOwnersByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<OwnersViewPerShop>> ListOwnersByShopNameOrId(string ListOwnersByShopNameOrShopId)
        {
            bool isNotNumber = !int.TryParse(ListOwnersByShopNameOrShopId, out int id) && ListOwnersByShopNameOrShopId != "0";
            string type = isNotNumber ? "name" : "ID";
            string userName = User.Identity.Name;

            var carWashShopEntity = await _dbContext.CarWashsShops
                .Include(x => x.Owners).ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Owners.Select(x => x.Owner.UserName).Contains(userName) && (x.Id == id || x.Name.ToUpper() == ListOwnersByShopNameOrShopId.ToUpper()));

            if (carWashShopEntity == null)
                return NotFound($"You don't have any CarWashShop with {type} '{ListOwnersByShopNameOrShopId}'..");

            var ownerView = _mapper.Map<OwnersViewPerShop>(carWashShopEntity);

            return Ok(ownerView);
        }



        //--3----------------------------------------------- GET ALL DISBAND REQUESTS ------------------------------------------------- 

        [HttpGet("GetAllDisbandRequests", Name = "getAllDisbandRequests")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<OwnerDisbandRequestView>>> GetAllDisbandRequests()
        {
            string userName = User.Identity.Name;

            var allDisbandRequestsEntities = await _dbContext.OwnerRemovalRequests
                .Include(x => x.Requester)
                .Include(x => x.CarWashShop)
                .Include(x => x.OwnerToBeRemoved)
                .Where(x => x.OwnerToBeRemoved.UserName == userName)
                .ToListAsync();

            if (allDisbandRequestsEntities == null || allDisbandRequestsEntities.Count == 0)
                return NotFound("There is no owner removal requests for you..");

            var allDisbandRequestsView = _mapper.Map<List<OwnerDisbandRequestView>>(allDisbandRequestsEntities);

            return Ok(allDisbandRequestsView);
        }



        //--4----------------------------------------------- GET DISBAND REQUEST BY 'ShopName' or 'ShopID' ------------------------------------------------- 

        [HttpGet("GetDisbandRequestByShop", Name = "getDisbandRequestByShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<OwnerDisbandRequestView>> GetDisbandRequestsByShop(string GetDisbandRequestByShop)
        {
            string userName = User.Identity.Name;
            bool isNotNumber = !int.TryParse(GetDisbandRequestByShop, out int id) && GetDisbandRequestByShop != "0";
            string type = isNotNumber ? "name" : "ID";

            var allDisbandRequestsEntities = await _dbContext.OwnerRemovalRequests
                .Include(x => x.Requester)
                .Include(x => x.CarWashShop)
                .Include(x => x.OwnerToBeRemoved)
                .FirstOrDefaultAsync(x => x.OwnerToBeRemoved.UserName == userName && (x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == GetDisbandRequestByShop.ToUpper()));


            if (allDisbandRequestsEntities == null)
                return NotFound($"There is no owner removal requests for you under the CarWashShop {type} {GetDisbandRequestByShop}..");

            var allDisbandRequestsView = _mapper.Map<OwnerDisbandRequestView>(allDisbandRequestsEntities);

            return Ok(allDisbandRequestsView);
        }



        //--5----------------------------------------------- GET ALL SHOP REMOVAL REQUESTS OF THE OWNER ------------------------------------------------- 

        [HttpGet("GetAllShopRemovalRequests", Name = "getAllShopRemovalRequests")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<OwnerShopRemovalRequestView>>> GetAllShopRemovalRequests()
        {
            string userName = User.Identity.Name;

            var allShopRemovalRequestsEntities = await _dbContext.ShopRemovalRequests
                .Include(x => x.Owner)
                .Include(x => x.CarWashShop)
                .Where(x => x.Owner.UserName == userName)
                .ToListAsync();


            if (allShopRemovalRequestsEntities == null || allShopRemovalRequestsEntities.Count == 0)
                return NotFound("There is no shop removal requests for you..");

            var allShopRemovalRequestsView = _mapper.Map<List<OwnerShopRemovalRequestView>>(allShopRemovalRequestsEntities);

            return Ok(allShopRemovalRequestsView);
        }



        //--6----------------------------------------------- GET SHOP REMOVAL REQUESTS OF THE OWNER BY 'ShopName' OR 'ShopID' ------------------------------------------------- 

        [HttpGet("GetShopRemovalByShop", Name = "getShopRemovalByShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<OwnerShopRemovalRequestView>> GetShopRemovalRequestsByShop(string GetShopRemovalByShop)
        {
            string userName = User.Identity.Name;
            bool isNotNumber = !int.TryParse(GetShopRemovalByShop, out int id) && GetShopRemovalByShop != "0";
            string type = isNotNumber ? "name" : "ID";

            var ShopRemovalRequestsEntity = await _dbContext.ShopRemovalRequests
                .Include(x => x.Owner)
                .Include(x => x.CarWashShop)
                .Where(x => x.Owner.UserName == userName && (x.CarWashShop.Id == id || x.CarWashShop.Name.ToUpper() == GetShopRemovalByShop.ToUpper()))
                .FirstOrDefaultAsync();


            if (ShopRemovalRequestsEntity == null)
                return NotFound($"There is no shop removal requests for you under the CarWashShop {type} {GetShopRemovalByShop}..");

            var ShopRemovalRequestsView = _mapper.Map<OwnerShopRemovalRequestView>(ShopRemovalRequestsEntity);

            return Ok(ShopRemovalRequestsView);
        }



        //--7---------------------------------- ADD NEW OWNERS TO CAR WASH SHOP IN USER'S POSSESSION BY 'ShopName' ------------------------------ 

        [HttpPost("AddOwnerToTheCarWashShopByShopNameOrShopId", Name = "addOwnerToTheCarWashShopByShopNameOrShopId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPost("OwnerRemovalRequest", Name = "ownerRemovalRequest")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPut("ApproveDisbandFromTheShop", Name = "approveDisbandFromTheShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPut("ApproveShopRemovalByShopNameOrID", Name = "ApproveShopRemovalByShopNameOrID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ApproveShopRemoval(string ApproveShopRemovalByShopNameOrID)
        {
            string userName = User.Identity.Name;
            bool isNotNumber = !int.TryParse(ApproveShopRemovalByShopNameOrID, out int id) && ApproveShopRemovalByShopNameOrID != "0";
            string type = isNotNumber ? "name" : "ID";

            var shopRemovalRequest = await _dbContext.ShopRemovalRequests
            .Include(x => x.Owner)
            .Include(x => x.CarWashShop)
            .Where(x => x.CarWashShopId == id || x.CarWashShop.Name.ToUpper() == ApproveShopRemovalByShopNameOrID.ToUpper())
            .ToListAsync();

            if (shopRemovalRequest.Count == 0)
                return NotFound($"CarWashShop with {type} '{ApproveShopRemovalByShopNameOrID}' doesn't exist..");

            bool isOwner = shopRemovalRequest.Any(x => x.Owner.UserName.Contains(userName));
            if (!isOwner)
                return BadRequest("You are not authorized to cancel this request..");

            var requestToApprove = shopRemovalRequest.FirstOrDefault(x => x.Owner.UserName == userName);
            requestToApprove.IsApproved = true;

            _dbContext.Entry(requestToApprove).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok($"You have approved removal of the CarWashShop '{requestToApprove.CarWashShop.Name}'!");
        }



        //--11----------------------------------------------- CANCEL CAR WASH SHOP REMOVAL REQUEST BY 'ShopName' OR 'ShopID' ------------------------------------------------- 

        [HttpDelete("CancelShopRemovalByShopNameOrID", Name = "cancelShopRemovalByShopNameOrID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
