using AutoMapper;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Controllers
{
    [Route("api/ConsumerManagement")]
    [ApiController]
    public class ConsumerManagementController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConsumerRepository _consumerRepository;

        public ConsumerManagementController(CarWashDbContext dbContext, IMapper mapper, IConsumerRepository consumerRepository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _consumerRepository = consumerRepository;
        }



        //--1----------------------------------------------- GET ALL SHOPS WITH FILTERS OR BY 'ShopID' -------------------------------------------------

        [HttpGet("GetAllShops-ConsumerSide", Name = "getAllShops-ConsumerSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> GetShops([FromQuery] CarWashFilter filter)
        {
            var carShopsEntities = await _consumerRepository.GetAllShops(filter);

            if (carShopsEntities == null || !carShopsEntities.Any())
                return NotFound("There is no CarWashShop found..");

            var shopsPaginated = await _consumerRepository.Pagination(HttpContext, carShopsEntities, filter.RecordsPerPage, filter.Pagination);

            var shopsView = _mapper.Map<List<CarWashShopView>>(shopsPaginated);

            return shopsView;
        }



        //--2---------------------------------------- GET ALL SERVICES -------------------------------------------

        [HttpGet("GetAllServices-ConsumerSide", Name = "getAllServices-ConsumerSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ServiceViewWithShopAssigned>>> GetServices([FromQuery] FilterServices filter)
        {
            string userName = User.Identity.Name;

            var serviceEntities = await _consumerRepository.GetAllServices(filter);

            if (serviceEntities == null || !serviceEntities.Any())
                return NotFound("There is no Service found..");

            var servicesPaginated = await _consumerRepository.Pagination(HttpContext, serviceEntities, filter.RecordsPerPage, filter.Pagination);

            var allServicesView = _mapper.Map<List<ServiceViewWithShopAssigned>>(servicesPaginated);

            return allServicesView;
        }



        //--3----------------------------------------------- FILTERED GET ALL BOOKINGS WITH  OR BY 'BookingID' -------------------------------------------------

        [HttpGet("GetAllBookings", Name = "getAllBookings")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Consumer")]
        public async Task<ActionResult<List<BookingViewConsumerSide>>> GetYourBookings([FromQuery] BookingFilters filter)
        {
            string userName = User.Identity.Name;

            var bookingsEntity = await _consumerRepository.GetAllBookings(userName, filter);

            if (bookingsEntity == null || !bookingsEntity.Any())
                return NotFound("No bookings found with specified filters");

            var bookingsPaginated = await _consumerRepository.Pagination(HttpContext, bookingsEntity, filter.RecordsPerPage, filter.Pagination);

            var bookingsView = _mapper.Map<List<BookingViewConsumerSide>>(bookingsPaginated);

            return bookingsView;
        }



        //--4----------------------------------------------- CREATE BOOKING FOR THE CAR WASH SERVICE -------------------------------------------------

        [HttpPost("ScheduleAService", Name = "scheduleAService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Consumer")]
        public async Task<ActionResult<BookingViewConsumerSide>> CreateBookng([FromBody] BookingCreation bookingCreation)
        {
            string userName = User.Identity.Name;
            string userId = await _consumerRepository.GetUserID(userName);
            int amountOfUsedWashingUnits = 0;

            var carWashShop = await _consumerRepository.GetShopToBookService(bookingCreation);

            if (carWashShop == null)
                return NotFound($"Car wash shop or service that you've selected doesn't exist" +
                                $"\nCheck your inputs:" +
                                $"\nCarwashID: '{bookingCreation.CarWashShopId}'" +
                                $"\nServiceID: '{bookingCreation.ServiceId}'");

            if (bookingCreation.ScheduledDateTime < DateTime.Now.AddHours(2))
                return BadRequest($"Booking needs to be scheduled at least 2 hours prior to the service start.." +
                                  $"\nCURRENT DATE: {DateTime.Now.Date.ToString("ddd, dd MMM yyyy")}" +
                                  $"\nCURRENT TIME: {DateTime.Now.ToString("HH:mm")}");

            carWashShop.Bookings.ForEach(x => { if(x.ScheduledDateTime == bookingCreation.ScheduledDateTime) { amountOfUsedWashingUnits++; } });

            if(amountOfUsedWashingUnits == carWashShop.AmountOfWashingUnits)
                return BadRequest($"Unfortunately all '{carWashShop.AmountOfWashingUnits}' washing units are already booked for the selected date and time..");

            if (!(bookingCreation.ScheduledDateTime.Hour >= carWashShop.OpeningTime && bookingCreation.ScheduledDateTime.Hour < carWashShop.ClosingTime))
                return BadRequest($"Your scheduled hour '{bookingCreation.ScheduledDateTime.ToString("HH:mm")}' is out of the '{carWashShop.Name}' working hours.." +
                                  $"\nOPENING TIME: {carWashShop.OpeningTime}" +
                                  $"\nCLOSING TIME: {carWashShop.ClosingTime}");

            var bookingEntity = _mapper.Map<Booking>(bookingCreation);
            bookingEntity.ConsumerId = userId;

            _dbContext.Bookings.Add(bookingEntity);
            await _dbContext.SaveChangesAsync();

            var bookingView = _mapper.Map<BookingViewConsumerSide>(bookingEntity);

            return bookingView;
        }



        //--5----------------------------------------------- CANCEL BOOKING BY 'BookingID' -------------------------------------------------

        [HttpDelete("cancelBookingById", Name = "cancelBookingById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Consumer")]
        public async Task<ActionResult<string>> CancelBooking(int bookingID)
        {
            string userName = User.Identity.Name;
            var bookingEntity = await _consumerRepository.GetBookingByID(bookingID, userName);

            if (bookingEntity != null)
            {
                var timeThresholdToCancelBooking = bookingEntity.ScheduledDateTime.AddMinutes(-15);
                if (DateTime.Now > timeThresholdToCancelBooking)
                    return BadRequest("You cannot cancel your booking less than 15 minutes before the scheduled time..");

                _dbContext.Bookings.Remove(bookingEntity);
                await _dbContext.SaveChangesAsync();

                return Ok($"You have successfully canceled your booking scheduled for '{bookingEntity.ScheduledDateTime.ToString("dddd, dd MMMM yyyy HH:mm")}'.");
            }
            return NotFound($"You don't have booking with ID: '{bookingID}'..");
        }
    }
}
