using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Tests.UnitTests
{
    [TestClass]
    public class OwnerManagementControllerTests : BaseTests
    {


        //--1--------------- GET FILTERED LIST OF OWNERS FOR EACH SHOP IN USER'S POSSESSION ---------------

        [TestMethod]
        public async Task GetListOfOwnersPerShopNoFilters()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new OwnersPerShopFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetOwners(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public async Task GetListOfOwnersOfShopById()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "msantos");
            var filter = new OwnersPerShopFilters() { CarWashShopId = 5};

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetOwners(filter);

            // Verification
            var result = response.Value;
            var owners = result.First();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, owners.Owners.Count());
        }


        [TestMethod]
        public async Task GetListOfOwnersNotFound()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "msantos");
            var filter = new OwnersPerShopFilters() { CarWashShopName = "CarWashShopName" };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = "No car wash shop found..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetOwners(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }



        //--2----------------------------- GET ALL BOOKINGS FOR THE SHOPS IN POSSESSION --------------------

        [TestMethod]
        public async Task GetListOfBookingsPerShopNoFilters()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllBookings(filter);

            // Verification
            var result = response.Value;
            var allShopsThatHaveBookings = result.Select(x => x.CarWashShopName).Distinct();

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count);
            Assert.AreEqual(2, allShopsThatHaveBookings.Count());
        }


        [TestMethod]
        public async Task GetListOfBookingsByShopId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            int shopId = 2;
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingFilters() { CarWashShopID = shopId };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllBookings(filter);

            // Verification
            var result = response.Value;
            var allShopsThatHaveBookings = result.Select(x => x.CarWashShopName).Distinct();

            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count);
        }

        [TestMethod]
        public async Task GetBookingsNotFound()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string shopName = "UnknownShopName";
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingFilters() { CarWashShopName = shopName };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = "No bookings found with specified filters";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllBookings(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(notFoundMessage, result.Value);
        }


        //--3-------------------------- GET ALL DISBAND REQUESTS --------------------------

        [TestMethod]
        public async Task GetAllDisbandRequestsNoFilters()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            var filter = new OwnerRequestsFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllDisbandRequests(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllDisbandRequestsFiltered()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "mohinder");
            var filter = new OwnerRequestsFilters() { NotApproved = true};

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllDisbandRequests(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllDisbandRequestsFilteredNotFound()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            var filter = new OwnerRequestsFilters() { NotApproved = true };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = "No disband request found..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetAllDisbandRequests(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(notFoundMessage, result.Value);
        }


        //--4---------------------------- GET ALL SHOP REMOVAL REQUESTS -------------------------

        [TestMethod]
        public async Task GetAllShopRemovalRequestsNoFilters()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            var filter = new OwnerRequestsFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetShopRemovalRequests(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetAllShopRemovalRequestsFiltered()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            var filter = new OwnerRequestsFilters() { NotApproved = true };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetShopRemovalRequests(filter);

            // Verification
            var result = response.Value;
            int shopId = result.Select(x => x.CarWashShopId).First();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, shopId);
        }

        [TestMethod]
        public async Task GetAllShopRemovalRequestsNotFound()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new OwnerRequestsFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = "There is no shop removal requests for you..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetShopRemovalRequests(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(notFoundMessage, result.Value);
        }


        //--5------------------------------- GET SHOP'S TOTAL REVENUE ----------------------------

        [TestMethod]
        public async Task GetShopTotalRevenue()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new RevenueFilters();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.GetTotalRevenue(filter);

            // Verification
            var result = response.Value;
            

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(40M, result[0].Revenue);
            Assert.AreEqual(10M, result[0].ByServicesRevenue[0].Revenue);
            Assert.AreEqual(30M, result[0].ByServicesRevenue[1].Revenue);
            Assert.AreEqual(16.25M, result[1].Revenue);
            Assert.AreEqual(0, result[1].ByServicesRevenue[0].Revenue);
            Assert.AreEqual(0, result[1].ByServicesRevenue[1].Revenue);
            Assert.AreEqual(16.25M, result[1].ByServicesRevenue[2].Revenue);
        }

        //--7----------------------- ADD NEW CO-OWNERS TO CAR WASH SHOP ------------------------------ 

        [TestMethod]
        public async Task AddNewCoOwnersToShopSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new CarWashShopOwnerAdd()
            {
                ShopId = 3,
                OwnerUserName = new List<string>()
                {
                    "mohinder",
                    "andry",
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have successfully added {filter.OwnerUserName.Count} more owners..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            int amountOfOwnersBefore = await dbContext.CarWashShopsOwners.Where(x => x.CarWashShopId == filter.ShopId).CountAsync();

            var response = await controller.AddNewCoOwnerToShop(filter);

            int amountOfOwnersAfter = await dbContext.CarWashShopsOwners.Where(x => x.CarWashShopId == filter.ShopId).CountAsync();
            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(1, amountOfOwnersBefore);
            Assert.AreEqual(3, amountOfOwnersAfter);

        }

        [TestMethod]
        public async Task IgnoringDuplicateAndFakeOwnersToShop()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new CarWashShopOwnerAdd()
            {
                ShopId = 2,
                OwnerUserName = new List<string>()
                {
                    "monica",       // duplicate
                    "mohinder",     // duplicate
                    "andry",        // legit new owner to be added
                    "vishnu",       // consumer
                    "fakeUserName"  // doesn't exist in Db
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have successfully added {filter.OwnerUserName.Count - 4} more owners..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            int amountOfOwnersBefore = await dbContext.CarWashShopsOwners.Where(x => x.CarWashShopId == filter.ShopId).CountAsync();

            var response = await controller.AddNewCoOwnerToShop(filter);

            int amountOfOwnersAfter = await dbContext.CarWashShopsOwners.Where(x => x.CarWashShopId == filter.ShopId).CountAsync();

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(2, amountOfOwnersBefore);
            Assert.AreEqual(3, amountOfOwnersAfter);
        }

        [TestMethod]
        public async Task FailToAddNewOwnerBadShopId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new CarWashShopOwnerAdd()
            {
                ShopId = 7,
                OwnerUserName = new List<string>()
                {
                    "monica",       // duplicate
                    "mohinder",     // duplicate
                    "andry",        // legit new owner to be added
                    "vishnu",       // consumer
                    "fakeUserName"  // doesn't exist in Db
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"CarWashShop with ID: '{filter.ShopId}' doesn't exist..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.AddNewCoOwnerToShop(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }


        [TestMethod]
        public async Task FailToAddNewOwnerAccessDeniedToShop()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new CarWashShopOwnerAdd()
            {
                ShopId = 5,
                OwnerUserName = new List<string>()
                {
                    "monica",       // duplicate
                    "mohinder",     // duplicate
                    "andry",        // legit new owner to be added
                    "vishnu",       // consumer
                    "fakeUserName"  // doesn't exist in Db
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            var selectedShop = await repository.GetCarWashShopToAssignOwners(filter.ShopId);

            string badReqMessage = $"You don't have access to '{selectedShop.Name}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.AddNewCoOwnerToShop(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        //--8---------------------- REQUEST OWNER DISBAND FROM THE SHOP ------------------------------ 

        [TestMethod]
        public async Task RequestOwnerDisbandSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "mohinder");
            var filter = new DisbandRequestCreation() { ShopId = 2,OwnerName = "monica"};
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == filter.ShopId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));


            string okMessage = $"Request to remove the owner '{filter.OwnerName}' from the '{shop.Name}' has been made by '{userName}', and now it needs to be approved..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.RequestOwnerRemoval(filter);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task OwnerDisbandRequestAlreadyExist()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new DisbandRequestCreation() { ShopId = 2, OwnerName = "mohinder" };
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == filter.ShopId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));


            string badReqMessage = $"Disband request for the owner '{filter.OwnerName}' from the shop '{shop.Name}' already exists..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.RequestOwnerRemoval(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public async Task FailedToDisbandForeignOwnerFromForeignShop()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new DisbandRequestCreation() {ShopId = 5, OwnerName = "alexp" };
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == filter.ShopId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"You don't have access to the CarWashShop '{shop.Name}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.RequestOwnerRemoval(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public async Task FailedToMatchOwnerWithShop()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new DisbandRequestCreation() { ShopId = 2, OwnerName = "alexp" };
 
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"Owner name '{filter.OwnerName}' doesn't match with the CarWashShop ID: '{filter.ShopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.RequestOwnerRemoval(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        [TestMethod]
        public async Task FailedToDisbandYourself()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new DisbandRequestCreation() { ShopId = 2, OwnerName = "monica" };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = "You cannot remove yourself..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.RequestOwnerRemoval(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }


        //--9-------------------------------- CONFIRM OR REJECT BOOKING --------------------------------

        [TestMethod]
        public async Task SuccessfullyConfirmedShopBooking()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingStatusSelection() { BookingId = 7, BookingStatus = BookingStatus.Confirmed};

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have {filter.BookingStatus} booking with ID: '{filter.BookingId}'";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ConfirmRejectBooking(filter);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }


        [TestMethod]
        public async Task TryingToConfirmAlreadyConfirmedBooking()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingStatusSelection() { BookingId = 6, BookingStatus = BookingStatus.Confirmed };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"Booking with ID '{filter.BookingId}' is already {filter.BookingStatus}..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ConfirmRejectBooking(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task TryingToRejectBookingLessThanHourBefore()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingStatusSelection() { BookingId = 4, BookingStatus = BookingStatus.Confirmed };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = "It needs to be at least 1 hour prior to the booking scheduled time";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ConfirmRejectBooking(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task FailedToConfirmBookingByBadId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new BookingStatusSelection() { BookingId = 99, BookingStatus = BookingStatus.Confirmed };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"There is no booking for your car wash shop with ID '{filter.BookingId}'";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ConfirmRejectBooking(filter);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }


        //--10--------------------------- APPROVE TO BE DISBANDED AS FROM THE SHOP ----------------------------------- 

        [TestMethod]
        public async Task SuccessfullyApprovedToGetDisbandFromShop()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "mohinder");
            int shopId = 2;
            var removalRequest = await repository.GetDisbandRequestToApprove(shopId, userName);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have approved to be disbanded from the CarWashShop '{removalRequest.CarWashShop.Name}'!";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ApproveDisbandFromShop(shopId);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task TryingToApproveDisbandRequestByBadShopId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "mohinder");
            int shopId = 10;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"There is no more disband requests to approve for the CarWashShop with ID '{shopId}'";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ApproveDisbandFromShop(shopId);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }


        //--11----------------------------------- APPROVE SHOP REMOVAL ------------------------------------------

        [TestMethod]
        public async Task SuccessfullyApprovedShopRemoval()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 1;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have approved removal of the CarWashShop 'CleanVehicleCenter'!";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ApproveShopRemoval(shopId);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }


        [TestMethod]
        public async Task FailedToApproveRemovalByBadId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 3;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"there is no more removal requests to approve for the CarWashShop with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ApproveShopRemoval(shopId);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task FailedToApproveAlreadyApprovedRemoval()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 5;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"there is no more removal requests to approve for the CarWashShop with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.ApproveShopRemoval(shopId);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }


        //--12-------------------------------- CANCEL SHOP REMOVAL REQUEST -------------------------------------

        [TestMethod]
        public async Task CancelShopRemoval()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 1;
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"Removal request of the '{shop.Name}' is successfully canceled";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CancelShopRemovalRequest(shopId);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
        }


        [TestMethod]
        public async Task FailedToCancelShopRemovalByWrongId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildOwnerRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 9;
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"Removal request for the CarWashShop with ID '{shopId}' doesn't exist..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerManagementController>>();
            var controller = new OwnerManagementController(mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.CancelShopRemovalRequest(shopId);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(notFoundMessage, result.Value);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
