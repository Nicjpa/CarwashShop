using CarWashShopAPI.Controllers;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashShopAPI.Tests.UnitTests
{
    [TestClass]
    public class ConsumerControllerTests : BaseTests
    {

        [TestMethod]
        public async Task GetAllShopsConsumer()
        {
            // Preparation
            string databaseName = Guid.NewGuid().ToString();
            var dbContext = BuildDbContext(databaseName);
            var mapper = BuildMapper();
            var repo = new Mock<IConsumerRepository>();

            


            dbContext.CarWashsShops.Add(new CarWashShop() 
            { 
                Name = "shop1",
                AdvertisingDescription = "BestInTown",
                AmountOfWashingUnits = 8,
                Address = "WallStreet",
                OpeningTime = 9,
                ClosingTime = 22
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Name = "shop2",
                AdvertisingDescription = "BestInTown",
                AmountOfWashingUnits = 10,
                Address = "WallStreet2",
                OpeningTime = 8,
                ClosingTime = 20
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Name = "shop3",
                AdvertisingDescription = "BestInTown",
                AmountOfWashingUnits = 5,
                Address = "WallStreet3",
                OpeningTime = 10,
                ClosingTime = 22
            });
            await dbContext.SaveChangesAsync();


            var dbContext2 = BuildDbContext(databaseName);
            var filter = new CarWashFilter()
            {
                Page = 1,
                RecordsPerPage = 10,
                CarWashName = "shop"
            };
            // Testing


            var controller = new ConsumerManagementController(dbContext2, mapper, repo.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            

            
            var response = await controller.GetShops(filter);

            // Verification
            var shops = response.Value;
            Assert.AreEqual(3, shops.Count);

        }
    }
}
