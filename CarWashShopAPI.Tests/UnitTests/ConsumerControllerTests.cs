using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Tests.UnitTests
{
    [TestClass]
    public class ConsumerControllerTests : BaseTests
    {

        //--1------- GET ALL SHOPS WITH FILTERS OR BY 'ShopID' ------------

        [TestMethod]
        public async Task GetAllShopsNoFilter()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new CarWashFilter();
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();
            string userName = await GetUserName(dbContext, "vishnu");
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, userName)
           }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetShops(filter);

            // Verification
            var result = response.Value;

            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public async Task GetAllShopsFilteredByIdSuccess()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new CarWashFilter() { CarWashShopId = 1 };
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();
            string userName = await GetUserName(dbContext, "vishnu");
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, userName)
           }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetShops(filter);

            // Verification
            var result = response.Value;

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllShopsFilteredByNameSuccess()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new CarWashFilter() { CarWashName = "Tsu" };
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();
            string userName = await GetUserName(dbContext, "vishnu");
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, userName)
           }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetShops(filter);

            // Verification
            var result = response.Value;

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result.First().Name, "Tsunami wash");
        }


        [TestMethod]
        public async Task GetAllShopsFilteredByNameFail()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new CarWashFilter() { CarWashName = "shop" };
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();
            string userName = await GetUserName(dbContext, "vishnu");
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, userName)
           }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetShops(filter);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
        }



        //--2-------------------------- GET ALL SERVICES -------------------------------

        [TestMethod]
        public async Task GetAllServicesNoFilter()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new FilterServices();
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();
            string userName = await GetUserName(dbContext, "vishnu");
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetServices(filter);

            // Verification
            var result = response.Value;

            Assert.AreEqual(12, result.Count);
        }

        [TestMethod]
        public async Task GetAllServicesFilteredByMinMaxPrice()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new FilterServices() { MinPrice = 10, MaxPrice = 15};
            string userName = await GetUserName(dbContext, "vishnu");
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));


            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetServices(filter);

            // Verification
            var result = response.Value;

            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public async Task GetAllServicesFilteredByIdFail()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new FilterServices() { ServiceID = 15 };
            string userName = await GetUserName(dbContext, "vishnu");
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.GetServices(filter);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
        }



        //--3----------------------------------------------- FILTERED GET ALL BOOKINGS WITH  OR BY 'BookingID' -------------------------------------------------

        [TestMethod]
        public async Task GetAllBookingsNoFilter()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new BookingFilters();
            string userName = await GetUserName(dbContext, "vishnu");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetYourBookings(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public async Task GetAllBookingsFilteredByConfirmedBooking()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new BookingFilters() { BookingStatus = BookingStatus.Confirmed};
            string userName = await GetUserName(dbContext, "cindy");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetYourBookings(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public async Task GetAllBookingsFilteredByUserWithNoBookings()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            var filter = new BookingFilters() { BookingStatus = BookingStatus.Confirmed };
            string userName = await GetUserName(dbContext, "alister");

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetYourBookings(filter);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
        }



        //--4---------------------------- CREATE BOOKING FOR THE CAR WASH SERVICE ----------------------------------

        [TestMethod]
        public async Task ScheduleBookingSuccessfully()
        {
            // Preparation

            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = DateTime.Today.AddDays(1).ToString().Substring(0, 10);

            var creation = new BookingCreation() 
            { 
                CarWashShopId = 1,
                ServiceId = 3,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = 12
            };
            

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.CarWashShopName, "CleanVehicleCenter");
            Assert.AreEqual(result.ServiceName, "LEGENDARY WASH");
            Assert.AreEqual(result.BookingStatus, BookingStatus.Pending);
        }


        [TestMethod]
        public async Task ScheduleBookingWithWrongShopID()
        {
            // Preparation

            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = DateTime.Today.AddDays(1).ToString().Substring(0, 10);

            var creation = new BookingCreation()
            {
                CarWashShopId = 15,
                ServiceId = 3,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = 12
            };


            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.IsFalse(dbContext.CarWashsShops.Any(x => x.Id == creation.CarWashShopId));
            Assert.IsTrue(dbContext.Services.Any(x => x.Id == creation.ServiceId));
            Assert.AreEqual(404, result2.StatusCode);
        }


        [TestMethod]
        public async Task ScheduleBookingWithWrongServiceID()
        {
            // Preparation

            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = DateTime.Today.AddDays(1).ToString().Substring(0, 10);

            var creation = new BookingCreation()
            {
                CarWashShopId = 2,
                ServiceId = 22,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = 12
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.IsTrue(dbContext.CarWashsShops.Any(x => x.Id == creation.CarWashShopId));
            Assert.IsFalse(dbContext.Services.Any(x => x.Id == creation.ServiceId));
            Assert.AreEqual(404, result2.StatusCode);
        }


        [TestMethod]
        public async Task ScheduleBookingToLatePriorToServiceStart()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = DateTime.Today.ToString().Substring(0, 10);
            int mockedCurrentHour = DateTime.Now.AddHours(2).Hour;
            var creation = new BookingCreation()
            {
                CarWashShopId = 2,
                ServiceId = 2,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = mockedCurrentHour,
            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"Booking needs to be scheduled at least 2 hours prior to the service start.." +
                                  $"\nCURRENT DATE: {DateTime.Now.Date.ToString("ddd, dd MMM yyyy")}" +
                                  $"\nCURRENT TIME: {DateTime.Now.ToString("HH:mm")}";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as BadRequestObjectResult;

            Assert.IsNull(result);
            Assert.IsTrue(dbContext.CarWashsShops.Any(x => x.Id == creation.CarWashShopId));
            Assert.IsTrue(dbContext.Services.Any(x => x.Id == creation.ServiceId));
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task ScheduleBookingOutOfWorkingHours()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = DateTime.Today.AddDays(1).ToString().Substring(0, 10);
            int shopId = 2;
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopId);

            var creation = new BookingCreation()
            {
                CarWashShopId = shopId,
                ServiceId = 2,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = 5,
            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"Your scheduled hour '{creation.ScheduledDateTime.ToString("HH:mm")}' is out of the '{shop.Name}' working hours.." +
                                  $"\nOPENING TIME: {shop.OpeningTime}" +
                                  $"\nCLOSING TIME: {shop.ClosingTime}";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as BadRequestObjectResult;

            Assert.IsNull(result);
            Assert.IsTrue(dbContext.CarWashsShops.Any(x => x.Id == creation.CarWashShopId));
            Assert.IsTrue(dbContext.Services.Any(x => x.Id == creation.ServiceId));
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task ScheduleBookingForTheHourWhenEverythingIsBooked()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alister");
            string mockedCurrentDate = new DateTime(2077, 5, 10, 15, 0, 0).ToString().Substring(0, 10);
            int shopId = 1;
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopId);

            var creation = new BookingCreation()
            {
                CarWashShopId = shopId,
                ServiceId = 3,
                ScheduledDate = mockedCurrentDate,
                ScheduledHour = 15,
            };
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"Unfortunately all '{shop.AmountOfWashingUnits}' washing units are already booked for the selected date and time..";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CreateBookng(creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as BadRequestObjectResult;

            Assert.IsNull(result);
            Assert.IsTrue(dbContext.CarWashsShops.Any(x => x.Id == creation.CarWashShopId));
            Assert.IsTrue(dbContext.Services.Any(x => x.Id == creation.ServiceId));
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task CancelBookingSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "cindy");
            int bookingId = 5;
            var booking = await dbContext.Bookings.FirstAsync(x => x.Id == bookingId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string OkReqMessage = $"You have successfully canceled your booking scheduled for '{booking.ScheduledDateTime.ToString("dddd, dd MMMM yyyy HH:mm")}'.";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            Assert.IsTrue(dbContext.Bookings.Any(x => x.Id == bookingId));
            var response = await controller.CancelBooking(bookingId);

            // Verification
            var result = response.Value;
            var result2 = response.Result as OkObjectResult;

            Assert.IsNull(result);
            Assert.IsFalse(dbContext.Bookings.Any(x => x.Id == bookingId));
            Assert.AreEqual(200, result2.StatusCode);
            Assert.AreEqual(OkReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task FailtToCancelBooking15MinBeforeTheStart()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "vishnu");
            int lateBookingId = 2;
            var booking = await dbContext.Bookings.FirstAsync(x => x.Id == lateBookingId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = "You cannot cancel your booking less than 15 minutes before the scheduled time..";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            Assert.IsTrue(dbContext.Bookings.Any(x => x.Id == lateBookingId));
            var response = await controller.CancelBooking(lateBookingId);

            // Verification
            var result = response.Value;
            var result2 = response.Result as BadRequestObjectResult;

            Assert.IsNull(result);
            Assert.IsTrue(dbContext.Bookings.Any(x => x.Id == lateBookingId));
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task CancelBookingWhichDoesntExistByID()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildConsumerRepo(dbContext);
            string userName = await GetUserName(dbContext, "vishnu");
            int unexsistingBookingId = 99;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"You don't have booking with ID: '{unexsistingBookingId}'..";
            var loggerMoq = Mock.Of<ILogger<ConsumerManagementController>>();

            // Testing
            var controller = new ConsumerManagementController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            Assert.IsFalse(dbContext.Bookings.Any(x => x.Id == unexsistingBookingId));
            var response = await controller.CancelBooking(unexsistingBookingId);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.IsFalse(dbContext.Bookings.Any(x => x.Id == unexsistingBookingId));
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }



    }
}
