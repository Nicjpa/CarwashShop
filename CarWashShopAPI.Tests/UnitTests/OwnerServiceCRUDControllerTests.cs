using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.ServiceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class OwnerServiceCRUDControllerTests : BaseTests
    {



        //--1-------------------------------- GET ALL SERVICES  --------------------------

        [TestMethod]
        public async Task GetAllServicesForEachShopNoFilters()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new FilterServices();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public async Task GetAllServicesNoShopCreatedNoServices()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "marktan");
            var filter = new FilterServices();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = "There is no Service with specified filter parameters..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }

        [TestMethod]
        public async Task GetAllServicesFilteredByShopName()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new FilterServices() { CarWashShopName = "Bubble" };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.All(x => x.Name.Contains("Bubble")));
        }

        [TestMethod]
        public async Task GetServiceFilteredByIDThatDoesntBelongToOwnersShop()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "monica");
            var filter = new FilterServices() { ServiceID = 10 };

            string notFoundMessage = "There is no Service with specified filter parameters..";

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Get(filter);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }



        //--2---------------------------------- ADD NEW SERVICE TO EXISTING SHOP --------------------------------------

        [TestMethod]
        public async Task CreateAndBoundServiceToShopSuccessfully()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "linda");
            int shopId = 4;
            var creation = new ServiceCreationAndUpdate()
            { 
                Name = "TestService",
                Description = "Just For Testing Purposes..",
                Price = 987.56M
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            int numOfServicesBefore = await dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop)
                .Where(x => x.CarWashShopId == shopId)
                .CountAsync();

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Post(shopId, creation);

            int numOfServicesAfter = await dbContext.CarWashShopsServices
                .Include(x => x.CarWashShop)
                .Where(x => x.CarWashShopId == shopId)
                .CountAsync();

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, numOfServicesBefore);
            Assert.AreEqual(4, numOfServicesAfter);
        }


        [TestMethod]
        public async Task FailToCreateServiceOnForeignShop()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "linda");
            int shopId = 1;
            var creation = new ServiceCreationAndUpdate()
            {
                Name = "TestService",
                Description = "Just For Testing Purposes..",
                Price = 987.56M
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            var shop = await dbContext.CarWashsShops
                .Include(x => x.Owners)
                .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == shopId);

            bool isLindaAmongOwners = shop.Owners.Any(x => x.Owner.UserName == userName);

            var badReqMessage = $"You don't have access to the {shop.Name} with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Post(shopId, creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as BadRequestObjectResult;

            Assert.IsNull(result);
            Assert.IsFalse(isLindaAmongOwners);
            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        [TestMethod]
        public async Task FailToCreateServiceOnUnexistingShop()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "linda");
            int shopId = 7;
            var creation = new ServiceCreationAndUpdate()
            {
                Name = "TestService",
                Description = "Just For Testing Purposes..",
                Price = 987.56M
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            bool doesShopExist = await dbContext.CarWashsShops.AnyAsync(x => x.Id == shopId);
            var notFoundMessage = $"There is no CarWashShop with ID: '{shopId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Post(shopId, creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.IsFalse(doesShopExist);
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }


        //--2----------------------------------- UPDATE SERVICE BY ID -----------------------------------

        [TestMethod]
        public async Task UpdateServiceByIDSuccessfully()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 10;
            var creation = new ServiceCreationAndUpdate()
            {
                Name = "Updated New Name",
                Description = "Updated New Description..",
                Price = 0.99M
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            var serviceToUpdate = await dbContext.Services.FirstOrDefaultAsync(x => x.Id == serviceId);
            string oldName = serviceToUpdate.Name;
            string oldDesc = serviceToUpdate.Description;
            decimal oldPrice = serviceToUpdate.Price;


            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Put(serviceId, creation);

            bool isNameChanged = oldName != serviceToUpdate.Name ? true : false;
            bool isDescChanged = oldDesc != serviceToUpdate.Description ? true : false;
            bool isPriceChanged = oldPrice != serviceToUpdate.Price ? true : false;

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.IsTrue(isNameChanged);
            Assert.IsTrue(isDescChanged);
            Assert.IsTrue(isPriceChanged);
        }


        [TestMethod]
        public async Task UpdateServiceByIDFailed()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 100;
            var creation = new ServiceCreationAndUpdate()
            {
                Name = "Updated New Name",
                Description = "Updated New Description..",
                Price = 0.99M
            };

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"You don't have any service with ID '{serviceId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Put(serviceId, creation);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }


        //--3----------------------------------- PATCH THE SERVICE ------------------------------

        [TestMethod]
        public async Task PatchServiceNameByIdSuccessfully()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 10;
            string servicePatchedName = "Patched Name";
            var patch = new JsonPatchDocument<ServiceCreationAndUpdate>();
            var readyPatch = patch.Replace(x => x.Name, servicePatchedName);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            controller.ObjectValidator = objectValidator.Object;


            var response = await controller.Patch(serviceId, readyPatch);

            // Verification
            var result = response.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, servicePatchedName);
        }


        [TestMethod]
        public async Task PatchServiceNameFailedWrongId()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 99;
            string servicePatchedName = "Patched Name";
            var patch = new JsonPatchDocument<ServiceCreationAndUpdate>();
            var readyPatch = patch.Replace(x => x.Name, servicePatchedName);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string notFoundMessage = $"You don't have any service with ID '{serviceId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };
            controller.ObjectValidator = objectValidator.Object;


            var response = await controller.Patch(serviceId, readyPatch);

            // Verification
            var result = response.Value;
            var result2 = response.Result as NotFoundObjectResult;

            Assert.IsNull(result);
            Assert.AreEqual(404, result2.StatusCode);
            Assert.AreEqual(notFoundMessage, result2.Value);
        }


        [TestMethod]
        public async Task PatchServiceFailedNull()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            var user = await dbContext.CustomUsers.Where(x => x.UserName == "alexp").FirstOrDefaultAsync();
            int serviceId = 10;
            string servicePatchedName = "Patched Name";
            JsonPatchDocument<ServiceCreationAndUpdate> patch = null;

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName)
            }));

            string badReqMessage = "You didn't specify which info do you want to patch..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Patch(serviceId, patch);

            // Verification
            var result2 = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result2.StatusCode);
            Assert.AreEqual(badReqMessage, result2.Value);
        }


        //--4----------------------------- REMOVE SERVICE FROM EXISTING SHOP -------------------------------------------

        [TestMethod]
        public async Task RemoveServiceSuccessfullyById()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 10;
            var service = await dbContext.Services
                .Include(x => x.CarWashShops)
                .ThenInclude(x => x.CarWashShop)
                .FirstOrDefaultAsync(x => x.Id == serviceId);

            string shopName = service.CarWashShops.Select(x => x.CarWashShop.Name).FirstOrDefault();

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string okMessage = $"You have successfully removed '{service.Name}' service from the {shopName} CarWashShop..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(serviceId);

            // Verification
            var result = response.Result as OkObjectResult;

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(okMessage, result.Value);
        }


        [TestMethod]
        public async Task RemoveServiceFailedBecuaseItsOnlyService()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "andry");
            int serviceId = 3;
            var carWashShop = await dbContext.CarWashsShops
                .Include(x => x.CarWashShopsServices)
                .FirstOrDefaultAsync(x => x.CarWashShopsServices.Any(x => x.ServiceId == serviceId));

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"You cannot delete the last and only existing service that you have in '{carWashShop.Name}' CarWashShop..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(serviceId);

            // Verification
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
            Assert.AreEqual(1, carWashShop.CarWashShopsServices.Count);
        }


        [TestMethod]
        public async Task RemoveServiceFailedByWrongId()
        {
            string databaseName = await PopulatedDataBase();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repository = BuildServiceRepo(dbContext);
            string userName = await GetUserName(dbContext, "alexp");
            int serviceId = 99;
            var carWashShop = await dbContext.Services.FirstOrDefaultAsync(x => x.Id == serviceId);

            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName)
            }));

            string badReqMessage = $"You don't have any service with ID '{serviceId}'..";

            // Testing
            var loggerMoq = Mock.Of<ILogger<OwnerServiceCRUDController>>();
            var controller = new OwnerServiceCRUDController(dbContext, mapper, repository, loggerMoq);
            controller.ModelState.Clear();
            controller.ControllerContext.HttpContext = new DefaultHttpContext() { User = userClaims };

            var response = await controller.Delete(serviceId);

            // Verification
            var result = response.Result as NotFoundObjectResult;

            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(badReqMessage, result.Value);
        }
    }
}
