using AutoMapper;
using CarWashShopAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashShopAPI.Tests.UnitTests
{
    public class AutoMapperTest
    {
        [TestClass]
        public class AutoMapperConfigurationTests
        {
            [TestMethod]
            public void AssertConfigurationIsValid()
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();
                });
                configuration.AssertConfigurationIsValid();
            }
        }
    }
}
