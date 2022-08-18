using AutoMapper;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Controllers
{
    [Route("api/Consumer")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConsumerController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        //--1----------------------------------------------- GET ALL SHOPS WITH FILTERS OR BY 'ShopID' -------------------------------------------------

        [HttpGet("GetFilteredAllShopsOrByShopID", Name = "getFilteredAllShopsOrByShopID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CarWashShopView>>> Get([FromQuery] CarWashFilter shopFilter)
        {
            var carShopsEntities = _dbContext.CarWashsShops
                .Include(a => a.CarWashShopsServices)
                .ThenInclude(b => b.Service)
                .AsQueryable();

            if (carShopsEntities == null || carShopsEntities.Count() == 0)
                return NotFound("You didn't create any CarWashShop yet..");

            if (shopFilter.Id != null)
            {
                carShopsEntities = carShopsEntities.Where(x => x.Id == shopFilter.Id);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(shopFilter.Name))
                    carShopsEntities = carShopsEntities.Where(x => x.Name.Contains(shopFilter.Name));

                if (!string.IsNullOrWhiteSpace(shopFilter.AdvertisingDescription))
                    carShopsEntities = carShopsEntities.Where(x => x.AdvertisingDescription.Contains(shopFilter.AdvertisingDescription));

                if (shopFilter.MinimumAmountOfWashingUnits != null)
                    carShopsEntities = carShopsEntities.Where(x => x.AmountOfWashingUnits >= shopFilter.MinimumAmountOfWashingUnits);

                if (shopFilter.RequiredAndEarlierOpeningTime != null)
                    carShopsEntities = carShopsEntities.Where(x => x.OpeningTime <= shopFilter.RequiredAndEarlierOpeningTime);

                if (shopFilter.RequiredAndLaterClosingTime != null)
                    carShopsEntities = carShopsEntities.Where(x => x.ClosingTime >= shopFilter.RequiredAndLaterClosingTime);

                if (!string.IsNullOrWhiteSpace(shopFilter.ServiceNameOrDescription))
                    carShopsEntities = carShopsEntities
                        .Where(x => x.CarWashShopsServices.Any(x => x.Service.Name.Contains(shopFilter.ServiceNameOrDescription)
                                 || x.Service.Description.Contains(shopFilter.ServiceNameOrDescription)));
            }

            if (carShopsEntities == null || carShopsEntities.Count() == 0)
                return NotFound("There is no CarWashShop with specified filter parameters..");

            await HttpContext.InsertPagination(carShopsEntities, shopFilter.RecordsPerPage);
            List<CarWashShop> listShopEntities = await carShopsEntities.Paginate(shopFilter.Pagination).ToListAsync();

            var shopsView = _mapper.Map<List<CarWashShopView>>(listShopEntities);

            return Ok(shopsView);
        }



        //--2----------------------------------------------- GET ALL BOOKINGS WITH FILTERS OR BY 'BookingID' -------------------------------------------------

        [HttpGet("GetFilteredAllBookingsOrByID", Name = "getFilteredAllBookingsOrByID")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ConsumerPolicy")]
        public async Task<ActionResult<List<BookingViewConsumerSide>>> GetYourBookings([FromQuery] BookingFilters bookingFilter)
        {
            string userName = User.Identity.Name;

            var bookingsEntity = _dbContext.Bookings
                .Include(x => x.CarWashShop)
                .Include(x => x.Service)
                .Include(x => x.Consumer)
                .Where(x => x.Consumer.UserName == userName)
                .OrderBy(x => x.ScheduledDateTime)
                .AsQueryable();

            if(bookingFilter.BookingID != null)
            {
                bookingsEntity = bookingsEntity.Where(x => x.Id == bookingFilter.BookingID);
            }
            else
            {
                if (bookingFilter.CarWashShopID != null)
                    bookingsEntity = bookingsEntity.Where(x => x.CarWashShopId == bookingFilter.CarWashShopID);

                if (bookingFilter.ServiceID != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ServiceId == bookingFilter.ServiceID);

                if(!string.IsNullOrWhiteSpace(bookingFilter.CarWashShopName))
                    bookingsEntity = bookingsEntity.Where(x => x.CarWashShop.Name == bookingFilter.CarWashShopName);

                if (!string.IsNullOrWhiteSpace(bookingFilter.ServiceName))
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Name == bookingFilter.ServiceName);

                if (bookingFilter.OnScheduledDate != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date == bookingFilter.OnScheduledDate);

                if (bookingFilter.ScheduledDatesBefore != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date < bookingFilter.ScheduledDatesBefore);

                if (bookingFilter.ScheduledDatesAfter != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Date > bookingFilter.ScheduledDatesAfter);

                if (bookingFilter.AtScheduledHour != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour == bookingFilter.AtScheduledHour);

                if (bookingFilter.ScheduledHoursBefore != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour < bookingFilter.ScheduledHoursBefore);

                if (bookingFilter.ScheduledHoursAfter != null)
                    bookingsEntity = bookingsEntity.Where(x => x.ScheduledDateTime.Hour > bookingFilter.ScheduledHoursAfter);

                if (bookingFilter.IsActiveBooking)
                    bookingsEntity = bookingsEntity.Where(x => !x.IsPaid);

                if (bookingFilter.IsConfirmed)
                    bookingsEntity = bookingsEntity.Where(x => x.BookingStatus == BookingStatus.Confirmed);

                if (bookingFilter.IsPending)
                    bookingsEntity = bookingsEntity.Where(x => x.BookingStatus == BookingStatus.Pending);

                if (bookingFilter.Price != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price == bookingFilter.Price);

                if (bookingFilter.MinPrice != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price >= bookingFilter.MinPrice);

                if (bookingFilter.MaxPrice != null)
                    bookingsEntity = bookingsEntity.Where(x => x.Service.Price <= bookingFilter.MaxPrice);
            }

            if (bookingsEntity == null || bookingsEntity.Count() == 0)
                return NotFound("No bookings found with specified filters");

            await HttpContext.InsertPagination(bookingsEntity, bookingFilter.RecordsPerPage);
            List<Booking> bookingEntities = await bookingsEntity.Paginate(bookingFilter.Pagination).ToListAsync();

            var bookingView = _mapper.Map<List<BookingViewConsumerSide>>(bookingEntities);

            return Ok(bookingView);
        }



        //--3----------------------------------------------- CREATE BOOKING FOR THE CAR WASH SERVICE -------------------------------------------------

        [HttpPost("ScheduleAService", Name = "scheduleAService")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ConsumerPolicy")]
        public async Task<ActionResult> Post([FromBody] BookingCreation bookingCreation)
        {
            string userName = User.Identity.Name;
            string userId = await _dbContext.CustomUsers.Where(x => x.UserName == userName).Select(x => x.Id).FirstOrDefaultAsync();
            int amountOfUsedWashingUnits = 0;

            var carWashShop = await _dbContext.CarWashsShops
                .Include(x => x.Bookings)
                .Include(x => x.CarWashShopsServices)
                .ThenInclude(x => x.Service)
                .FirstOrDefaultAsync(x => x.Id == bookingCreation.CarWashShopId && x.CarWashShopsServices
                                    .Select(x => x.ServiceId)
                                    .Contains(bookingCreation.ServiceId));

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

            if (!(bookingCreation.ScheduledDateTime.Hour >= carWashShop.OpeningTime && bookingCreation.ScheduledDateTime.Hour < carWashShop.ClosingTime)
                || amountOfUsedWashingUnits == carWashShop.AmountOfWashingUnits)
                return BadRequest("There is no available schedule for the selected date and time..");

            var bookingEntity = _mapper.Map<Booking>(bookingCreation);
            bookingEntity.ConsumerId = userId;

            var bookedServiceName = carWashShop.CarWashShopsServices
                .Where(x => x.ServiceId == bookingCreation.ServiceId)
                .Select(x => x.Service.Name)
                .FirstOrDefault();

            _dbContext.Bookings.Add(bookingEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have booked your service '{bookedServiceName}', it requires confirmation.." +
                      $"\nSHOP NAME: '{carWashShop.Name}'" +
                      $"\nDATE: '{bookingEntity.ScheduledDateTime.Date.ToString("ddd, dd MMM yyyy")}'" +
                      $"\nTIME: {bookingEntity.ScheduledDateTime.Hour}:00");
        }



        //--4----------------------------------------------- CANCEL BOOKING BY 'BookingID' -------------------------------------------------

        [HttpDelete("{id:int}", Name = "cancelBookingById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ConsumerPolicy")]
        public async Task<ActionResult> Delete(int id)
        {
            string userName = User.Identity.Name;
            var bookingEntity = await _dbContext.Bookings.Include(x => x.Consumer).FirstOrDefaultAsync(x => x.Id == id && x.Consumer.UserName == userName);

            if (bookingEntity != null)
            {
                _dbContext.Bookings.Remove(bookingEntity);
                await _dbContext.SaveChangesAsync();

                var timeThresholdToCancelBooking = bookingEntity.ScheduledDateTime.AddMinutes(-15);
                if(DateTime.Now > timeThresholdToCancelBooking)
                    return BadRequest("You cannot cancel your booking less than 15 minutes before the scheduled time..");

                return Ok($"You have successfully canceled your booking scheduled for '{bookingEntity.ScheduledDateTime.ToString("dddd, dd MMMM yyyy HH:mm")}'. ");
            }
            return NotFound($"You don't have booking with ID: '{id}'..");
        }





    }
}
