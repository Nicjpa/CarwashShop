using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
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
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
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

            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
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
        public async Task CreateNewShopWithServicesSuccessfully()
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
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
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
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
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
                }
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
            }));

            string badReqMessage = $"CarWashShop '{shop.Name}' already exists..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            var response = await controller.Post(filter);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }


        //--4-------------------------- UPDATE SHOP'S GENERAL INFO ---------------------------------------------

        [TestMethod]
        public async Task UpdateShopInfoSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            int shopId = 1;

            var update = new CarWashShopUpdate()
            {
                Name = "Updated Shop Name",
                AdvertisingDescription = "some description",
                AmountOfWashingUnits = 16,
                Address = "Somewhere Over The Rainbow 99",
                OpeningTime = 14,
                ClosingTime = 18
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Put(shopId, update);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, update.Name);
            Assert.AreEqual(result.AdvertisingDescription, update.AdvertisingDescription);
            Assert.AreEqual(result.AmountOfWashingUnits, update.AmountOfWashingUnits);
            Assert.AreEqual(result.Address, update.Address);
            Assert.AreEqual(result.OpeningTime, update.OpeningTime);
            Assert.AreEqual(result.ClosingTime, update.ClosingTime);
        }


        [TestMethod]
        public async Task UpdateShopInfoFailedDuplicatedName()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            var shop = await dbContext.CarWashsShops.FirstOrDefaultAsync();
            int shopId = 1;

            var update = new CarWashShopUpdate()
            {
                Name = shop.Name,
                AdvertisingDescription = "some description",
                AmountOfWashingUnits = 16,
                Address = "Somewhere Over The Rainbow 99",
                OpeningTime = 14,
                ClosingTime = 18
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
            }));

            string badReqMessage = $"CarWashShop '{update.Name}' already exists..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Put(shopId, update);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }


        [TestMethod]
        public async Task UpdateShopInfoFailedBadShopId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            int shopId = 101;

            var update = new CarWashShopUpdate()
            {
                Name = "AquaStrike",
                AdvertisingDescription = "some description",
                AmountOfWashingUnits = 16,
                Address = "Somewhere Over The Rainbow 99",
                OpeningTime = 14,
                ClosingTime = 18
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
            }));

            string notFoundMessage = $"You don't have any CarWashShop with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Put(shopId, update);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(notFoundMessage, result.Value);
        }


        //--5-------------------------------------- PATCH SHOP'S INFO  ------------------------------------------

        [TestMethod]
        public async Task PatchShopByIdSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 5;
            string shopPatchedName = "Patched Name";
            var patch = new JsonPatchDocument<CarWashShopUpdate>();
            var readyPatch = patch.Replace(x => x.Name, shopPatchedName);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            controller.ObjectValidator = objectValidator.Object;


            var response = await controller.Patch(shopId, readyPatch);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, shopPatchedName);
        }


        [TestMethod]
        public async Task PatchShopFailedNullJsonDoc()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 5;
            JsonPatchDocument<CarWashShopUpdate> patch = null;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));

            string badReqMessage = "You didn't specify which info do you want to patch..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Patch(shopId, patch);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }


        [TestMethod]
        public async Task PatchShopFailedBadId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 99;
            string shopPatchedName = "Patched Name";
            var patch = new JsonPatchDocument<CarWashShopUpdate>();
            var readyPatch = patch.Replace(x => x.Name, shopPatchedName);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));

            string badReqMessage = $"You don't have any CarWashShop with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Patch(shopId, readyPatch);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }


        //--6---------------------------- REMOVE THEB SHOP --------------------------------

        [TestMethod]
        public async Task DeleteShopByIdSuccessfully()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "linda");
            int shopId = 4;
            var deleteStatement = new CarWashShopRemovalRequestCreation();
            var shopName = await dbContext.CarWashsShops.FirstOrDefaultAsync(x => x.Id == shopId);
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));
            string okMessage = $"You have successfully removed '{shopName.Name}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(shopId, deleteStatement);

            bool isShopDeleted = await dbContext.CarWashsShops.AnyAsync(x => x.Id == shopId);
            
            // Verification
            var result = response.Result as OkObjectResult;

            Assert.AreEqual(okMessage, result.Value);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsFalse(isShopDeleted);
        }


        [TestMethod]
        public async Task MakeRemovalRequestCoownership()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "mohinder");
            int shopId = 2;
            var deleteStatement = new CarWashShopRemovalRequestCreation();
            var shopName = await dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == shopId);

            string otherOwners = await repository.ConcatenateCoOwnerNames(shopName, userName);
            int numOfOtherOwners = shopName.Owners.Count(x => x.Owner.UserName != userName);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));

            string okMessage = $"Removal request has been made, because you are sharing ownership of the '{shopName.Name}' with {otherOwners}and now it awaits their approval..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(shopId, deleteStatement);

            bool doesShopExist = await dbContext.CarWashsShops.AnyAsync(x => x.Id == shopId);
            bool requestsAreMade = await dbContext.ShopRemovalRequests.Where(x => x.CarWashShopId == shopId).CountAsync() == numOfOtherOwners ? true : false;

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.AreEqual(okMessage, result.Value);
            Assert.IsTrue(doesShopExist);
            Assert.IsTrue(requestsAreMade);
        }


        [TestMethod]
        public async Task DeleteShopFailedByBadId()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "linda");
            int shopId = 99;
            var deleteStatement = new CarWashShopRemovalRequestCreation();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));
            string badReqMessage = $"There is no CarWashShop with ID: '{shopId}' in your possession..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(shopId, deleteStatement);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task RemovalRequestsAreAlreadyMade()
        {
            // Preparation
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildCarWashRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int shopId = 5;
            var deleteStatement = new CarWashShopRemovalRequestCreation();
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));
            string badReqMessage = $"Removal request is already made for the CarWashShop with ID: '{shopId}'";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerCarWashShopCRUDController>>();
            var controller = new OwnerCarWashShopCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            await controller.Delete(shopId, deleteStatement);

            var response = await controller.Delete(shopId, deleteStatement);

            bool requestsAlreadyExists = await dbContext.ShopRemovalRequests.AnyAsync(x => x.CarWashShopId == shopId);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(400, result.StatusCode);
            Assert.IsTrue(requestsAlreadyExists);
        }
    }
}
