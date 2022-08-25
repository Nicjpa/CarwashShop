using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarWashShopAPI.Tests.UnitTests
{
    [TestClass]
    public class OwnerCarWashShopCRUDControllerTests : BaseTests
    {

        //--1------------------------------ GET ALL EXISTING SHOPS IN POSSESSION ---------------------- 

        [TestMethod]
        public async Task GetAllShopsInPossessionNoFilter()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new CarWashFilter();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public async Task GetShopInPossessionById()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            int shopId = 3;
            var filter = new CarWashFilter() { CarWashShopId = shopId };
            
            var shop = await dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == shopId);


            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(shop.Owners.Any(x => x.Owner.UserName == "monica"));
        }


        //--3------------------------------ CREATE NEW SHOP WITH NEW SERVICES -------------------------------------- 

        [TestMethod]
        public async Task CreateNewShopWithServices()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            string shopName = "New Created Shop name";

            var filter = new CarWashShopCreation()
            {
                Name = shopName,
                AdvertisingDescription = "some commercial text",
                Address = "Mars Street B3/7",
                OpeningTime = 9,
                ClosingTime = 21,
                Services = new List<ServiceCreationAndUpdate>()
                {
                    new ServiceCreationAndUpdate() { Name = "New Service 1", Description = "desc", Price = 5.5M},
                    new ServiceCreationAndUpdate() { Name = "New Service 2", Description = "desc", Price = 15.5M},
                    new ServiceCreationAndUpdate() { Name = "New Service 3", Description = "desc", Price = 25.5M},
                },
                CarWashShopsOwners = new List<string>()
                {
                    "linda", "marktan", "unexistingUserName"
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Post(filter);

            var newShop = await dbContext.CarWashsShops
                .Include(x => x.Owners)
                .FirstOrDefaultAsync(x => x.Name == shopName);

            int numberOfCoOwners = newShop.Owners.Count();

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Services.Count);
            Assert.AreEqual(3, numberOfCoOwners);
        }


        [TestMethod]
        public async Task CreateNewShopFailedBecauseOfNoServicesAdded()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            string shopName = "New Created Shop name";

            var filter = new CarWashShopCreation()
            {
                Name = shopName,
                AdvertisingDescription = "some commercial text",
                Address = "Mars Street B3/7",
                OpeningTime = 9,
                ClosingTime = 21,
                Services = new List<ServiceCreationAndUpdate>(),
                CarWashShopsOwners = new List<string>()
                {
                    "linda", "marktan", "unexistingUserName"
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = "Your shop needs to have at least one washing service..";

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Post(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }


        [TestMethod]
        public async Task CreateNewShopFailedDuplicatedShopName()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync();

            var filter = new CarWashShopCreation()
            {
                Name = shop.Name,
                AdvertisingDescription = "some commercial text",
                Address = "Mars Street B3/7",
                OpeningTime = 9,
                ClosingTime = 21,
                Services = new List<ServiceCreationAndUpdate>()
                {
                    new ServiceCreationAndUpdate() { Name = "New Service 1", Description = "desc", Price = 5.5M},
                    new ServiceCreationAndUpdate() { Name = "New Service 2", Description = "desc", Price = 15.5M},
                    new ServiceCreationAndUpdate() { Name = "New Service 3", Description = "desc", Price = 25.5M},
                },
                CarWashShopsOwners = new List<string>()
                {
                    "linda", "marktan", "unexistingUserName"
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
            }));

            string badReqMessage = $"CarWashShop '{shop.Name}' already exists..";

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Post(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }
    }
}
