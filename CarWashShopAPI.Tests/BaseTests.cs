using AutoMapper;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashShopAPI.Tests
{
    public class BaseTests
    {
        protected CarWashDbContext BuildDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<CarWashDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new CarWashDbContext(options);
            return dbContext;
        }

        protected IMapper BuildMapper()
        {
            var config = new MapperConfiguration(opt => 
            {
                opt.AddProfile(new AutoMapperProfile());
            });
            return config.CreateMapper();
        }
    }
}
