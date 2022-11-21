using CarWashShopAPI.DTO.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Tests.UnitTests
{
    [TestClass]
    public class AccountControllerTests : BaseTests
    {

        [TestMethod]
        public async Task UserIsCreated()
        {
            var databaseName = await PopulatedDataBase();
            await CreateAnUser(databaseName);
            var context2 = BuildDbContext(databaseName);
            var count = await context2.Users.CountAsync();
            Assert.AreEqual(11, count);
        }

        [TestMethod]
        public async Task UserCannotLogin()
        {
            var databaseName = await PopulatedDataBase();
            await CreateAnUser(databaseName);

            var controller = BuildAccountsController(databaseName);
            var userInfo = new UserLogin() 
            {
                UserName = "linda",
                Password = "Mercury"
            };
            var response = await controller.Login(userInfo);

            Assert.IsNull(response.Value);

            var result = response.Result as BadRequestObjectResult;
            Assert.AreEqual(result.StatusCode, 400);
        }

        [TestMethod]
        public async Task UserCanLogin()
        {
            var databaseName = await PopulatedDataBase();
            await CreateAnUser(databaseName);

            var controller = BuildAccountsController(databaseName);
            var userInfo = new UserLogin()
            {
                UserName = "linda",
                Password = "ValueShore3!"
            };
            var response = await controller.Login(userInfo);

            Assert.IsNotNull(response.Value);
        }


        private async Task CreateAnUser(string databaseName)
        {
            var accountController = BuildAccountsController(databaseName);
            var userInfo = new UserInfo() 
            {
                FirstName = "Linda",
                LastName = "Jones",
                Address = "Summerville Lane 19",
                UserName = "linda",
                Email = "linda.jones@gmail.com",
                PhoneNumber = "+1 223-814-3940",
                Password = "Mercury!",
                Role = RoleClaim.Owner
            };
            await accountController.CreateUser(userInfo);
        }


        
    }
}
