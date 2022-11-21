using AutoMapper;
using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarWashShopAPI.Controllers
{
    [Route("api/ConsumerManagement")]
    [ApiController]
    public class ConsumerManagementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConsumerRepository _consumerRepository;
        private readonly ILogger<ConsumerManagementController> _logger;
        public ConsumerManagementController
            (
            IMapper mapper, 
            IConsumerRepository consumerRepository, 
            ILogger<ConsumerManagementController> logger
            )
        {
            _mapper = mapper;
            _consumerRepository = consumerRepository;
            _logger = logger;
        }



        //--1----------------------------------------------- GET ALL SHOPS WITH FILTERS OR BY 'ShopID' -------------------------------------------------

        [HttpGet("GetAllShops-ConsumerSide", Name = "getAllShops-ConsumerSide")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> GetShops([FromQuery] CarWashFilter filter)
        {
            string userName = User.Identity.Name;
            
            var carShopsEntities = await _consumerRepository.GetAllShops(filter);

            if (carShopsEntities == null || !carShopsEntities.Any())
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShops-ConsumerSide' / '0' SHOPS FOUND ");
                return NotFound("There is no CarWashShop found..");
            }

            var shopsPaginated = await _consumerRepository.Pagination(HttpContext, carShopsEntities, filter.RecordsPerPage, new PaginationDTO { Page = filter.Page, RecordsPerPage = filter.RecordsPerPage });

            var shopsView = _mapper.Map<List<CarWashShopView>>(shopsPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetShops-ConsumerSide' / '{shopsView.Count}' SHOPS FOUND");
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
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetServices-ConsumerSide' / '0' SERVICES FOUND ");
                return NotFound("There is no Service found..");
            }

            var servicesPaginated = await _consumerRepository.Pagination(HttpContext, serviceEntities, filter.RecordsPerPage, new PaginationDTO { Page = filter.Page, RecordsPerPage = filter.RecordsPerPage });

            var allServicesView = _mapper.Map<List<ServiceViewWithShopAssigned>>(servicesPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetServices-ConsumerSide' / '{allServicesView.Count}' SERVICES FOUND");
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
            {
                _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetBookings-ConsumerSide' / '0' BOOKINGS FOUND ");
                return NotFound("No bookings found with specified filters");
            }

            var bookingsPaginated = await _consumerRepository.Pagination(HttpContext, bookingsEntity, filter.RecordsPerPage, new PaginationDTO { Page = filter.Page, RecordsPerPage = filter.RecordsPerPage });

            var bookingsView = _mapper.Map<List<BookingViewConsumerSide>>(bookingsPaginated);

            _logger.LogInformation($" / GET / UserName: '{userName.ToUpper()}' / MethodName: 'GetBookings-ConsumerSide' / '{bookingsView.Count}' BOOKINGS FOUND");
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
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'ScheduleService-ConsumerSide' " +
                    $"/ shop and service didn't match / BOOKING FAILED ");

                return NotFound($"Car wash shop or service that you've selected doesn't exist" +
                                $"\nCheck your inputs:" +
                                $"\nCarwashID: '{bookingCreation.CarWashShopId}'" +
                                $"\nServiceID: '{bookingCreation.ServiceId}'");
            }

            if (bookingCreation.ScheduledDateTime < DateTime.Now.AddHours(2))
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'ScheduleService-ConsumerSide' " +
                    $"/ attempt to schedule booking less than 2h before / BOOKING FAILED ");

                return BadRequest($"Booking needs to be scheduled at least 2 hours prior to the service start.." +
                                  $"\nCURRENT DATE: {DateTime.Now.Date.ToString("ddd, dd MMM yyyy")}" +
                                  $"\nCURRENT TIME: {DateTime.Now.ToString("HH:mm")}");
            }

            carWashShop.Bookings.ForEach(x => { if(x.ScheduledDateTime == bookingCreation.ScheduledDateTime) { amountOfUsedWashingUnits++; } });

            if (amountOfUsedWashingUnits == carWashShop.AmountOfWashingUnits)
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'ScheduleService-ConsumerSide' " +
                    $"/ everything is booked already for the selected time / BOOKING FAILED ");

                return BadRequest($"Unfortunately all '{carWashShop.AmountOfWashingUnits}' washing units are already booked for the selected date and time..");
            }

            if (!(bookingCreation.ScheduledDateTime.Hour >= carWashShop.OpeningTime && bookingCreation.ScheduledDateTime.Hour < carWashShop.ClosingTime))
            {
                _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'ScheduleService-ConsumerSide' " +
                    $"/ selected time is out of working hours / BOOKING FAILED ");

                return BadRequest($"Your scheduled hour '{bookingCreation.ScheduledDateTime.ToString("HH:mm")}' is out of the '{carWashShop.Name}' working hours.." +
                                  $"\nOPENING TIME: {carWashShop.OpeningTime}" +
                                  $"\nCLOSING TIME: {carWashShop.ClosingTime}");
            }

            var bookingEntity = _mapper.Map<Booking>(bookingCreation);
            bookingEntity.ConsumerId = userId;

            await _consumerRepository.AddBooking(bookingEntity);
            await _consumerRepository.Commit();

            var bookingView = _mapper.Map<BookingViewConsumerSide>(bookingEntity);

            _logger.LogInformation($" / POST / UserName: '{userName.ToUpper()}' / MethodName: 'ScheduleService-ConsumerSide' / BOOKING SUCCESSFULL ");
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
                {
                    _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'CancelBooking-ConsumerSide' " +
                        $"/ attempted to cancel booking less than 15 minutes before / CANCELATION FAILED ");

                    return BadRequest("You cannot cancel your booking less than 15 minutes before the scheduled time..");
                }

                await _consumerRepository.DeleteBooking(bookingEntity);
                await _consumerRepository.Commit();

                _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'CancelBooking-ConsumerSide' " +
                        $"/ SUCCESSFULLY CANCELED ");

                return Ok($"You have successfully canceled your booking scheduled for '{bookingEntity.ScheduledDateTime.ToString("dddd, dd MMMM yyyy HH:mm")}'.");
            }

            _logger.LogInformation($" / DELETE / UserName: '{userName.ToUpper()}' / MethodName: 'CancelBooking-ConsumerSide' " +
                        $"/ booking doesn't exist, wrong booking ID / CANCELATION FAILED ");

            return NotFound($"You don't have booking with ID: '{bookingID}'..");
        }
    }
}
