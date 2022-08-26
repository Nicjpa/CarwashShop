using AutoMapper;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CarWashShopAPI.Controllers
{
    [Route("api/OwnerManagement")]
    [ApiController]
    public class OwnerManagementController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILogger<OwnerManagementController> _logger;

        public OwnerManagementController(
            CarWashDbContext dbContext, 
            IMapper mapper, 
            IOwnerRepository ownerRepository,
            ILogger<OwnerManagementController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _logger = logger;
        }



        //--1------------------------------ GET FILTERED LIST OF OWNERS FOR EACH SHOP IN USER'S POSSESSION ----------------------  

        [HttpGet("GetShopOwners", Name = "getShopOwners")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<ListOfOwnersPerShopView>>> GetOwners([FromQuery] ListOfOwnersPerShopFilters filters)
        {
            string userName = User.Identity.Name;

            var carWashShopEntities = await _ownerRepository.GetOwners(userName, filters);

            if (carWashShopEntities == null || !carWashShopEntities.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetOwners-OwnerSide' " +
                    $"/ no filtered results / '0' SHOPS FOUND ");
                return NotFound("No car wash shop found..");
            }

            var ownersPaginated = await _ownerRepository.Pagination(HttpContext, carWashShopEntities, filters.RecordsPerPage, filters.Pagination);

            var ownersView = _mapper.Map<List<ListOfOwnersPerShopView>>(ownersPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetOwners-OwnerSide' " +
                    $"/ '{ownersView.Count}' SHOPS FOUND ");
            return ownersView;
        }



        //--2----------------------------------------------- GET ALL BOOKINGS FOR THE SHOPS IN POSSESSION -------------------------------------------------

        [HttpGet("GetShopBookings", Name = "getShopBookings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<BookingViewOwnerSide>>> GetAllBookings([FromQuery] BookingFilters filter)
        {
            string userName = User.Identity.Name;

            var bookingsEntity = await _ownerRepository.GetBookings(userName, filter);

            if (bookingsEntity == null || !bookingsEntity.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopBookings-OwnerSide' " +
                    $"/ no filtered results / '0' BOOKINGS FOUND ");
                return NotFound("No bookings found with specified filters");
            }
            var bookingsPaginated = await _ownerRepository.Pagination(HttpContext, bookingsEntity, filter.RecordsPerPage, filter.Pagination);

            var bookingsView = _mapper.Map<List<BookingViewOwnerSide>>(bookingsPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopBookings-OwnerSide' " +
                    $"/ '{bookingsView.Count}' BOOKINGS FOUND ");
            return bookingsView;
        }
        


        //--3----------------------------------------------- GET ALL DISBAND REQUESTS ------------------------------------------------- 

        [HttpGet("GetDisbandRequests", Name = "getDisbandRequests")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<DisbandRequestView>>> GetAllDisbandRequests([FromQuery] OwnerRequestsFilters filters)
        {
            string userName = User.Identity.Name;

            var allDisbandRequestsEntities = await _ownerRepository.GetDisbandRequests(userName, filters);

            if (allDisbandRequestsEntities == null || !allDisbandRequestsEntities.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetDisbandRequests-OwnerSide' " +
                   $"/ no filtered results / '0' DISBAND REQUESTS FOUND ");
                return NotFound("No disband request found..");
            }
            var disbandRequestsPaginated = await _ownerRepository.Pagination(HttpContext, allDisbandRequestsEntities, filters.RecordsPerPage, filters.Pagination);

            var allDisbandRequestsView = _mapper.Map<List<DisbandRequestView>>(disbandRequestsPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetDisbandRequests-OwnerSide' " +
                   $"/ '{allDisbandRequestsView.Count}' DISBAND REQUESTS FOUND ");
            return allDisbandRequestsView;
        }



        //--4----------------------------------------------- GET ALL SHOP REMOVAL REQUESTS ------------------------------------------------- 

        [HttpGet("GetShopRemovalRequests", Name = "getShopRemovalRequests")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<ShopRemovalRequestView>>> GetShopRemovalRequests([FromQuery] OwnerRequestsFilters filters)
        {
            string userName = User.Identity.Name;

            var allShopRemovalRequestsEntities = await _ownerRepository.GetShopRemovalRequests(userName, filters);

            if (allShopRemovalRequestsEntities == null || !allShopRemovalRequestsEntities.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopRemovalRequests-OwnerSide' " +
                   $"/ no filtered results / '0' SHOP REMOVAL REQUESTS FOUND ");

                return NotFound("There is no shop removal requests for you..");
            }
            var removalRequestsPaginated = await _ownerRepository.Pagination(HttpContext, allShopRemovalRequestsEntities, filters.RecordsPerPage, filters.Pagination);

            var allShopRemovalRequestsView = _mapper.Map<List<ShopRemovalRequestView>>(removalRequestsPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopRemovalRequests-OwnerSide' " +
                   $"/ '{allShopRemovalRequestsView.Count}' SHOP REMOVAL REQUESTS FOUND ");

            return allShopRemovalRequestsView;
        }



        //--5----------------------------------------------- GET SHOP'S TOTAL REVENUE -------------------------------------------------

        [HttpGet("GetRevenue", Name = "getRevenue")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<RevenueReportView>>> GetTotalRevenue([FromQuery] RevenueFilters revenueFilters)
        {
            string userName = User.Identity.Name;

            var carWashShops = await _ownerRepository.GetShopsForRevenue(userName, revenueFilters);

            if (!carWashShops.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopRevenue-OwnerSide' " +
                   $"/ no filtered results / '0' SHOP'S REVENUE REPORTS FOUND ");
                return NotFound("No Revenue found..");
            }
            var allRevenueReports = await _ownerRepository.CalculateRevenue(carWashShops);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopRevenue-OwnerSide' " +
                   $"/ '{allRevenueReports.Count}' SHOP'S REVENUE REPORTS FOUND ");
            return allRevenueReports;
        }



        //--6----------------------------------------------- GET OVERVIEW OF INCOME FOR THE SHOPS -------------------------------------------------

        [HttpGet("GetIncomeReport", Name = "getIncomeReport")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult> GetDailyWeeklyMonthlyYearlyIncom([FromQuery] IncomeFilter filter)
        {
            string userName = User.Identity.Name;

            var incomeEntities = await _ownerRepository.GetIncome(filter, userName);

            if (incomeEntities.Count() == 0)
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopIncome-OwnerSide' " +
                   $"/ no filtered results / '0' SHOP'S INCOME REPORTS FOUND ");
                return NotFound($"Either there was no income in '{filter.ForTheYear}', or there is no CarWashShop with ID '{filter.CarWashShopID}' in your possesion..");
            }

            if(filter.CalendarFormat.ToString() == "Day")
            {
                var incomeDays = await _ownerRepository.IncomeEntityMap2IncomeViewDays(incomeEntities, filter);
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopIncome-OwnerSide' " +
                   $"/ '{incomeDays.Count}' SHOP'S INCOME REPORTS FOUND ");
                return Ok(incomeDays);
            }
            else
            {
                var incomeOther = await _ownerRepository.IncomeEntityMap2IncomeViewOther(incomeEntities, filter);
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShopIncome-OwnerSide' " +
                  $"/ '{incomeOther.Count}' SHOP'S INCOME REPORTS FOUND ");
                return Ok(incomeOther);
            }
        }



        //--7---------------------------------- ADD NEW CO-OWNERS TO CAR WASH SHOP ------------------------------ 

        [HttpPost("AddOwnerToShop", Name = "addOwnerToShop")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> AddNewCoOwnerToShop(int shopID, [FromBody] CarWashShopOwnerAdd newOwners)
        {
            string userName = User.Identity.Name;

            var carWashShop = await _ownerRepository.GetCarWashShopToAssignOwners(shopID);

            if (carWashShop == null)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewOwnersToShop-OwnerSide' " +
                   $"/ no owner added, bad shop ID '{shopID}' / ADDING NEW OWNERS FAILED ");
                return NotFound($"CarWashShop with ID: '{shopID}' doesn't exist..");
            }

            var currentOwnerList = carWashShop.Owners.Select(x => x.Owner.UserName).ToList();
            if (!currentOwnerList.Contains(userName))
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewOwnersToShop-OwnerSide' " +
                   $"/ invalid attempt to access foreign shop with ID '{shopID}' / ADDING NEW OWNERS FAILED ");
                return BadRequest($"You don't have access to '{carWashShop.Name}'..");
            }
            var CurrentOwnerUserIds = new List<string>();
            carWashShop.Owners.ForEach(x => CurrentOwnerUserIds.Add(x.Owner.Id));

            var approvedOwnersIDs = await _ownerRepository.GetApprovedOwnerIDs(newOwners, currentOwnerList);

            var legitNewOwners = await _ownerRepository.AssignNewOwnersToTheShop(carWashShop, approvedOwnersIDs, CurrentOwnerUserIds);

            string message = legitNewOwners.Any() ? $"You have successfully added {approvedOwnersIDs.Count} more owners.." : "No new owner added..";

            _dbContext.CarWashShopsOwners.AddRange(legitNewOwners);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'AddNewOwnersToShop-OwnerSide' " +
                   $"/ new '{approvedOwnersIDs.Count}' owners added successfully / ADDING NEW OWNERS SUCCESS");
            return Ok(message);
        }



        //--8---------------------------------- REQUEST OWNER DISBAND FROM THE SHOP ------------------------------ 

        [HttpPost("RequestOwnerRemoval", Name = "requestOwnerRemoval")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> RequestOwnerRemoval(int shopID,[FromBody] DisbandRequestCreation ownerRemovalRequest)
        {
            string userName = User.Identity.Name;
            string ownerNameToRemove = ownerRemovalRequest.OwnerName.ToLower();

            if (userName == ownerRemovalRequest.OwnerName.ToLower())
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'DisbandRequest-OwnerSide' " +
                   $"/ attempt to disband ownself / DISBAND REQUEST FAILED ");

                return BadRequest("You cannot remove yourself..");
            }
                

            var carWashShop = await _ownerRepository.GetShopToDisbandOwner(shopID, ownerNameToRemove);

            if (carWashShop == null)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'DisbandRequest-OwnerSide' " +
                   $"/ owner name '{ownerNameToRemove}' and shop ID '{shopID}' do not match / DISBAND REQUEST FAILED ");

                return BadRequest($"Owner name '{ownerNameToRemove}' doesn't match with the CarWashShop ID: '{shopID}'..");
            }
            else
            {
                bool isUserOwner = carWashShop.Owners.Select(x => x.Owner.UserName).Contains(userName);
                if (!isUserOwner)
                {
                    _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'DisbandRequest-OwnerSide' " +
                   $"/ invalid attempt to access foreign shop with ID '{shopID}' / DISBAND REQUEST FAILED ");

                    return BadRequest($"You don't have access to the CarWashShop '{carWashShop.Name}'..");
                }
            }

            bool isRequestMadeAlready = carWashShop.DisbandRequests.Any(x => x.OwnerToBeRemoved.UserName.Contains(ownerNameToRemove));
            if (isRequestMadeAlready)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'DisbandRequest-OwnerSide' " +
                   $"/ disband request for the '{ownerNameToRemove}' from '{carWashShop.Name}' already exists / DISBAND REQUEST FAILED ");

                return BadRequest($"Disband request for the owner '{ownerNameToRemove}' from the shop '{carWashShop.Name}' already exists..");
            }

            var removalRequest = await _ownerRepository.CreateDisbandRequest(ownerRemovalRequest, carWashShop, userName);

            _dbContext.OwnerRemovalRequests.Add(removalRequest);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'DisbandRequest-OwnerSide' " +
                   $"/ disband request for the '{ownerNameToRemove}' from '{carWashShop.Name}' / DISBAND REQUEST SUCCESSFULLY MADE ");

            return Ok($"Request to remove the owner '{ownerNameToRemove}' from the '{carWashShop.Name}' has been made by '{userName}', and now it needs to be approved..");
        }



        //--9----------------------------------------------- CONFIRM OR REJECT BOOKING -------------------------------------------------

        [HttpPut("ConfirmRejectBooking")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> ConfirmRejectBooking([FromQuery] BookingStatusSelection status)
        {
            string userName = User.Identity.Name;

            var bookingEntity = await _ownerRepository.GetBookingByID(status, userName);

            if (bookingEntity == null)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'Confirm/RejectBooking-OwnerSide' " +
                   $"/ no booking found for the shop with ID '{status.BookingId}' / CONFIRM/REJECT FAILED ");

                return NotFound($"There is no booking for your car wash shop with ID '{status.BookingId}'");
            }
            if (DateTime.Now.AddHours(1) > bookingEntity.ScheduledDateTime)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'Confirm/RejectBooking-OwnerSide' " +
                   $"/ less than an hour to change booking status / CONFIRM/REJECT FAILED ");

                return BadRequest("It needs to be at least 1 hour prior to the booking scheduled time");
            }

            if (bookingEntity.BookingStatus != status.BookingStatus)
            {
                bookingEntity.BookingStatus = status.BookingStatus;

                _dbContext.Entry(bookingEntity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'Confirm/RejectBooking-OwnerSide' " +
                   $"/ booking status changed to '{bookingEntity.BookingStatus}' with ID '{status.BookingId}' / CONFIRM/REJECT SUCCESS ");

                return Ok($"You have {bookingEntity.BookingStatus} booking with ID: '{status.BookingId}'");
            }

            _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'Confirm/RejectBooking-OwnerSide' " +
                   $"/ attempt to overwrite booking status '{bookingEntity.BookingStatus}' with a same status / CONFIRM/REJECT FAILED ");

            return BadRequest($"Booking with ID '{status.BookingId}' is already {bookingEntity.BookingStatus}..");
        }



        //--10----------------------------------------------- APPROVE TO BE DISBANDED AS FROM THE SHOP ------------------------------------------------- 

        [HttpPut("ApproveDisband", Name = "approveDisband")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> ApproveDisbandFromShop(int shopID)
        {
            string userName = User.Identity.Name;

            var ownerRemovalRequest = await _ownerRepository.GetDisbandRequestToApprove(shopID, userName);

            if (ownerRemovalRequest == null)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'ApproveDisband-OwnerSide' " +
                   $"/ no disband request for the shop with ID '{shopID}' / DISBAND APPROVAL FAILED ");

                return NotFound($"There is no more disband requests to approve for the CarWashShop with ID '{shopID}'");
            }
            ownerRemovalRequest.IsApproved = true;

            _dbContext.Entry(ownerRemovalRequest).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'ApproveDisband-OwnerSide' " +
                  $"/ approved disband from the shop with ID '{shopID}' / DISBAND APPROVAL SUCCESS ");

            return Ok($"You have approved to be disbanded from the CarWashShop '{ownerRemovalRequest.CarWashShop.Name}'!");
        }



        //--11----------------------------------------------- APPROVE SHOP REMOVAL ------------------------------------------------- 

        [HttpPut("ApproveShopRemoval", Name = "ApproveShopRemoval")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> ApproveShopRemoval(int shopID)
        {
            string userName = User.Identity.Name;

            var shopRemovalRequest = await _ownerRepository.GetShopRequestsToApprove(shopID, userName);

            if (shopRemovalRequest.Count == 0)
            {
                _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'ApproveShopRemoval-OwnerSide' " +
                  $"/ no shop removal request found for the shop with ID '{shopID}' / SHOP REMOVAL FAILED ");

                return NotFound($"there is no more removal requests to approve for the CarWashShop with ID: '{shopID}'..");
            }

            var requestToApprove = shopRemovalRequest.FirstOrDefault(x => x.Owner.UserName == userName);
            requestToApprove.IsApproved = true;

            _dbContext.Entry(requestToApprove).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / PUT / UserName: '{userName.ToUpper()}' / MethodName: 'ApproveShopRemoval-OwnerSide' " +
                  $"/ approved shop removal of the '{requestToApprove.CarWashShop.Name}' with ID '{shopID}' / SHOP REMOVAL APPROVED ");

            return Ok($"You have approved removal of the CarWashShop '{requestToApprove.CarWashShop.Name}'!");
        }



        //--12------------------------------------------------ CANCEL SHOP REMOVAL REQUEST ------------------------------------------------- 

        [HttpDelete("CancelShopRemoval", Name = "cancelShopRemoval")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<string>> CancelShopRemovalRequest(int shopID)
        {
            string userName = User.Identity.Name;
            var cwShopRemovalRequests = await _ownerRepository.GetShopRequestsToCancel(shopID, userName);

            if (cwShopRemovalRequests == null || cwShopRemovalRequests.Count == 0)
            {
                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'CancelShopRemoval-OwnerSide' " +
                  $"/ no removal request for the shop with ID '{shopID}' / SHOP REMOVAL CANCELATION FAILED ");

                return NotFound($"Removal request for the CarWashShop with ID '{shopID}' doesn't exist..");
            }

            _dbContext.ShopRemovalRequests.RemoveRange(cwShopRemovalRequests);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'CancelShopRemoval-OwnerSide' " +
                  $"/ shop removal of the '{cwShopRemovalRequests[0].CarWashShop.Name}' with ID '{shopID}' has been canceled / SHOP REMOVAL IS CANCELED ");

            return Ok($"Removal request of the '{cwShopRemovalRequests[0].CarWashShop.Name}' is successfully canceled");
        }
    }
}
