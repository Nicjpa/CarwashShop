using AutoMapper;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CarWashShopAPI.DTO.Enums;

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

        protected async Task<string> GetUserName(CarWashDbContext dbContext, string userName)
        {
             var user = await dbContext.CustomUsers
                .Where(x => x.UserName == userName)
                .FirstOrDefaultAsync();
            
            return user.UserName;
        }

        protected IConsumerRepository BuildConsumerRepo(CarWashDbContext dbContext)
        {
            var repo = new ConsumerRepository(dbContext);
            return repo;
        }

        protected IServiceRepository BuildServiceRepo(CarWashDbContext dbContext)
        {
            var repo = new ShopServiceRepository(dbContext);
            return repo;
        }

        protected ICarWashRepository BuildCarWashRepo(CarWashDbContext dbContext)
        {
            var repo = new CarWashRepository(dbContext);
            return repo;
        }

        public async Task<string> PopulatedDataBase()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            var dbContext = BuildDbContext(dataBaseName);
            var hasher = new PasswordHasher<CustomUser>();
            var password = "ValueShore3!";

            // CAR WASH SHOPS
            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Id = 1,
                Name = "CleanVehicleCenter",
                AdvertisingDescription = "BestInTown",
                AmountOfWashingUnits = 2,
                Address = "WallStreet",
                OpeningTime = 9,
                ClosingTime = 22
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Id = 2,
                Name = "Tsunami wash",
                AdvertisingDescription = "BestInTown",
                AmountOfWashingUnits = 10,
                Address = "WallStreet2",
                OpeningTime = 8,
                ClosingTime = 20
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Id = 3,
                Name = "BubbleTime",
                AdvertisingDescription = "Refresh your vehicle",
                AmountOfWashingUnits = 5,
                Address = "Pinapple Block 82",
                OpeningTime = 8,
                ClosingTime = 23
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Id = 4,
                Name = "Emerald Wash",
                AdvertisingDescription = "Your car's appearance matters",
                AmountOfWashingUnits = 10,
                Address = "Sunrise Hill 206",
                OpeningTime = 8,
                ClosingTime = 23
            });

            dbContext.CarWashsShops.Add(new CarWashShop()
            {
                Id = 5,
                Name = "Geyser Blaze",
                AdvertisingDescription = "Thorough and professional cleaning",
                AmountOfWashingUnits = 8,
                Address = "Black desert street 75",
                OpeningTime = 8,
                ClosingTime = 23
            });


            // SERVICES
            dbContext.Services.Add(new Service()
            {
                Id = 1,
                Name = "STANDARD",
                Description = "RegularService",
                Price = 10M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 2,
                Name = "PREMIUM",
                Description = "MaxService",
                Price = 15M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 3,
                Name = "LEGENDARY WASH",
                Description = "MarvelousService",
                Price = 50M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 4,
                Name = "Emerald BRIGHT",
                Description = "desc",
                Price = 12.50M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 5,
                Name = "Emerald SHINE",
                Description = "desc",
                Price = 16.75M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 6,
                Name = "Emerald DIVINE",
                Description = "desc",
                Price = 24.99M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 7,
                Name = "Bubble CASUAL",
                Description = "desc",
                Price = 7.99M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 8,
                Name = "Bubble EXTRA",
                Description = "desc",
                Price = 12M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 9,
                Name = "Bubble FLOOD",
                Description = "desc",
                Price = 16.25M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 10,
                Name = "Geyser LIGHT",
                Description = "desc",
                Price = 10.70M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 11,
                Name = "Geyser MEDIUM",
                Description = "desc",
                Price = 15.25M
            });

            dbContext.Services.Add(new Service()
            {
                Id = 12,
                Name = "Geyser BLAZE",
                Description = "desc",
                Price = 18.75M
            });

            // SHOPS_SERVICES
            dbContext.CarWashShopsServices.AddRange(new List<CarWashShopsServices>()
            {
                new CarWashShopsServices() {CarWashShopId = 1, ServiceId = 3},
                new CarWashShopsServices() {CarWashShopId = 2, ServiceId = 1},
                new CarWashShopsServices() {CarWashShopId = 2, ServiceId = 2},
                new CarWashShopsServices() {CarWashShopId = 3, ServiceId = 7},
                new CarWashShopsServices() {CarWashShopId = 3, ServiceId = 8},
                new CarWashShopsServices() {CarWashShopId = 3, ServiceId = 9},
                new CarWashShopsServices() {CarWashShopId = 4, ServiceId = 4},
                new CarWashShopsServices() {CarWashShopId = 4, ServiceId = 5},
                new CarWashShopsServices() {CarWashShopId = 4, ServiceId = 6},
                new CarWashShopsServices() {CarWashShopId = 5, ServiceId = 10},
                new CarWashShopsServices() {CarWashShopId = 5, ServiceId = 11},
                new CarWashShopsServices() {CarWashShopId = 5, ServiceId = 12},
            });

            // SHOPS_OWNERS
            dbContext.CarWashShopsOwners.AddRange(new List<CarWashShopsOwners>()
            { 
                new CarWashShopsOwners() {CarWashShopId = 1, OwnerId = "edebb245-2066-4126-b9e4-dc020ffdafe7" },
                new CarWashShopsOwners() {CarWashShopId = 1, OwnerId = "e8952694-1ca9-44b1-a8fa-73988bb4eee5"},
                new CarWashShopsOwners() {CarWashShopId = 2, OwnerId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f"},
                new CarWashShopsOwners() {CarWashShopId = 3, OwnerId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f"},
                new CarWashShopsOwners() {CarWashShopId = 4, OwnerId = "71a07f92-c8b6-47a8-8f1f-0eb340062e57"},
                new CarWashShopsOwners() {CarWashShopId = 5, OwnerId = "f4352621-5ced-4afa-854f-49a10819d206"},
                new CarWashShopsOwners() {CarWashShopId = 5, OwnerId = "74ea7ef1-0444-447a-9780-0b3a0126a20b"},
                new CarWashShopsOwners() {CarWashShopId = 5, OwnerId = "f02b000c-622d-4c3f-b215-7e08cea2469c"},

            });

            var ownerClaim1 = new IdentityUserClaim<string>()
            {
                Id = 1,
                UserId = "edebb245-2066-4126-b9e4-dc020ffdafe7",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim2 = new IdentityUserClaim<string>()
            {
                Id = 2,
                UserId = "e8952694-1ca9-44b1-a8fa-73988bb4eee5",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim3 = new IdentityUserClaim<string>()
            {
                Id = 3,
                UserId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim4 = new IdentityUserClaim<string>()
            {
                Id = 4,
                UserId = "71a07f92-c8b6-47a8-8f1f-0eb340062e57",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim5 = new IdentityUserClaim<string>()
            {
                Id = 5,
                UserId = "f4352621-5ced-4afa-854f-49a10819d206",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim6 = new IdentityUserClaim<string>()
            {
                Id = 6,
                UserId = "6f57119a-6b89-43ed-8df4-4b70d5259548",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim7 = new IdentityUserClaim<string>()
            {
                Id = 7,
                UserId = "f02b000c-622d-4c3f-b215-7e08cea2469c",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim8 = new IdentityUserClaim<string>()
            {
                Id = 8,
                UserId = "74ea7ef1-0444-447a-9780-0b3a0126a20b",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            dbContext.UserClaims.AddRange(new List<IdentityUserClaim<string>>()
            {
                ownerClaim1, ownerClaim2, ownerClaim3, ownerClaim4, 
                ownerClaim5, ownerClaim6, ownerClaim7, ownerClaim8
            });



            dbContext.CustomUsers.AddRange(new List<CustomUser>
            {
                // CONSUMERS
                new CustomUser()
                {
                    Id = "a73fc0f6-3559-4848-9224-099903fcdca2",
                    FirstName = "Cindy",
                    LastName = "Shaw",
                    Address = "Rapsberry Grow 154",
                    Role = "Consumer",
                    UserName = "cindy",
                    NormalizedUserName = "CINDY",
                    Email = "cindy.shaw@gmail.com",
                    NormalizedEmail = "CINDY.SHAW@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 582-333-4023"
                },

                new CustomUser()
                {
                    Id = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                    FirstName = "Vishnu",
                    LastName = "Gawas",
                    Address = "22nd Jump Street",
                    Role = "Consumer",
                    UserName = "vishnu",
                    NormalizedUserName = "VISHNU",
                    Email = "vishnu.gawas@gmail.com",
                    NormalizedEmail = "VISHNU.GAWAS@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 213-354-2486"
                },

                new CustomUser()
                {
                    Id = "989b1e73-da14-4218-ac8c-d60aaf816520",
                    FirstName = "Alister",
                    LastName = "Fernandez",
                    Address = "Palmville Heights 24",
                    Role = "Consumer",
                    UserName = "alister",
                    NormalizedUserName = "ALISTER",
                    Email = "alister.fernandez@gmail.com",
                    NormalizedEmail = "ALISTER.FERNANDEZ@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 508-796-7186"
                },

                // OWNERS
                new CustomUser()
                {
                    Id = "edebb245-2066-4126-b9e4-dc020ffdafe7",
                    FirstName = "Andry",
                    LastName = "Goncharenko",
                    Address = "Yellow Roof Street 66",
                    Role = "Owner",
                    UserName = "andry",
                    NormalizedUserName = "ANDRY",
                    Email = "andry.goncharenko@gmail.com",
                    NormalizedEmail = "ANDRY.GONCHARENKO@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 410-470-4327"
                },

                new CustomUser()
                {
                    Id = "e8952694-1ca9-44b1-a8fa-73988bb4eee5",
                    FirstName = "Mohinder",
                    LastName = "Pathania",
                    Address = "Barksdale Boulevard 506",
                    Role = "Owner",
                    UserName = "mohinder",
                    NormalizedUserName = "MOHINDER",
                    Email = "mohinder.pathania@gmail.com",
                    NormalizedEmail = "MOHINDER.PATHANIA@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 509-243-9105"
                },

                new CustomUser()
                {
                    Id = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f",
                    FirstName = "Monica",
                    LastName = "Bordei",
                    Address = "Silverlake Lane 96",
                    Role = "Owner",
                    UserName = "monica",
                    NormalizedUserName = "MONICA",
                    Email = "monica.bordei@gmail.com",
                    NormalizedEmail = "MONICA.BORDEI@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 582-322-0444"
                },

                new CustomUser()
                {
                    Id = "71a07f92-c8b6-47a8-8f1f-0eb340062e57",
                    FirstName = "Linda",
                    LastName = "Jones",
                    Address = "Summerville Lane 19",
                    Role = "Owner",
                    UserName = "linda",
                    NormalizedUserName = "LINDA",
                    Email = "linda.jones@gmail.com",
                    NormalizedEmail = "LINDA.JONES@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 223-814-3940"
                },

                new CustomUser()
                {
                    Id = "f4352621-5ced-4afa-854f-49a10819d206",
                    FirstName = "Michael",
                    LastName = "Santos",
                    Address = "Iron Boulevard 45",
                    Role = "Owner",
                    UserName = "msantos",
                    NormalizedUserName = "MSANTOS",
                    Email = "michael.santos@gmail.com",
                    NormalizedEmail = "MICHAEL.SANTOS@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 262-589-1904"
                },

                new CustomUser()
                {
                    Id = "6f57119a-6b89-43ed-8df4-4b70d5259548",
                    FirstName = "Mark",
                    LastName = "Tanarte",
                    Address = "66th Street",
                    Role = "Owner",
                    UserName = "marktan",
                    NormalizedUserName = "MARKTAN",
                    Email = "mark.tanarte@gmail.com",
                    NormalizedEmail = "MARK.TANARTE@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 409-861-1005"
                },

                new CustomUser()
                {
                    Id = "f02b000c-622d-4c3f-b215-7e08cea2469c",
                    FirstName = "Alex",
                    LastName = "Petcu",
                    Address = "Timberwood Fall 64",
                    Role = "Owner",
                    UserName = "alexp",
                    NormalizedUserName = "ALEXP",
                    Email = "alex.petcu@gmail.com",
                    NormalizedEmail = "ALEX.PETCU@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 423-923-5656"
                },

                new CustomUser()
                {
                    Id = "74ea7ef1-0444-447a-9780-0b3a0126a20b",
                    FirstName = "Ramon",
                    LastName = "Altamiranda",
                    Address = "Sintagma Square 106",
                    Role = "Owner",
                    UserName = "altaramon",
                    NormalizedUserName = "ALTARAMON",
                    Email = "ramon.altamiranda@gmail.com",
                    NormalizedEmail = "RAMON.ALTAMIRANDA@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = hasher.HashPassword(null, password),
                    PhoneNumber = "+1 505-753-6592"
                },

            });


            // BOOKINGS
            dbContext.Bookings.AddRange(new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                    ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                    CarWashShopId = 1,
                    ServiceId  = 3,
                    ScheduledDateTime = new DateTime(2077, 5, 10, 15, 0, 0),
                    IsPaid = true,
                    BookingStatus = BookingStatus.Confirmed
                },

                new Booking()
                {
                    Id = 2,
                    ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                    CarWashShopId = 2,
                    ServiceId  = 1,
                    ScheduledDateTime = DateTime.Now,
                    IsPaid = false,
                    BookingStatus = BookingStatus.Rejected
                },

                new Booking()
                {
                    Id = 3,
                    ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                    CarWashShopId = 2,
                    ServiceId  = 2,
                    ScheduledDateTime = new DateTime(2022, 10, 05, 11, 0, 0),
                    IsPaid = false,
                    BookingStatus = BookingStatus.Pending
                },

                new Booking()
                {
                    Id = 4,
                    ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                    CarWashShopId = 2,
                    ServiceId  = 2,
                    ScheduledDateTime = new DateTime(2022, 9, 7, 19, 0, 0),
                    IsPaid = false,
                    BookingStatus = BookingStatus.Pending
                },



                new Booking()
                {
                    Id = 5,
                    ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                    CarWashShopId = 1,
                    ServiceId  = 3,
                    ScheduledDateTime = new DateTime(2077, 5, 10, 15, 0, 0),
                    IsPaid = true,
                    BookingStatus = BookingStatus.Confirmed
                },

                new Booking()
                {
                    Id = 6,
                    ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                    CarWashShopId = 2,
                    ServiceId  = 1,
                    ScheduledDateTime = new DateTime(2022, 10, 10, 15, 0, 0),
                    IsPaid = false,
                    BookingStatus = BookingStatus.Confirmed
                },

                new Booking()
                {
                    Id = 7,
                    ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                    CarWashShopId = 2,
                    ServiceId  = 2,
                    ScheduledDateTime = new DateTime(2022, 10, 05, 11, 0, 0),
                    IsPaid = false,
                    BookingStatus = BookingStatus.Pending
                },

                new Booking()
                {
                    Id = 8,
                    ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                    CarWashShopId = 2,
                    ServiceId  = 2,
                    ScheduledDateTime = new DateTime(2022, 9, 7, 19, 0, 0),
                    IsPaid = false,
                    BookingStatus = BookingStatus.Pending
                },

            });
            await dbContext.SaveChangesAsync();
            return dataBaseName;
        }
        //protected ControllerContext BuildControllerContextWithDefaultUser()
        //{
        //    var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //    {
        //        new Claim(ClaimTypes.Role, "Consumer")
        //    }, "test"));

        //    return new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = user }
        //    };
        //}


        //protected WebApplicationFactory<Program> BuildWebApplicationFactory(string databaseName, bool bypassSecurity = true)
        //{
        //    var factory = new WebApplicationFactory<Program>();

        //    factory = factory.WithWebHostBuilder(builder =>
        //    {
        //        builder.ConfigureTestServices(services =>
        //        {
        //            var descriptorDbContext = services.SingleOrDefault(d =>
        //            d.ServiceType == typeof(DbContextOptions<CarWashDbContext>));

        //            if (descriptorDbContext != null)
        //            {
        //                services.Remove(descriptorDbContext);
        //            }

        //            services.AddDbContext<CarWashDbContext>(options =>
        //            {
        //                options.UseInMemoryDatabase(databaseName);
        //            });

        //            if (bypassSecurity)
        //            {
        //                services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();

        //                services.AddControllers(options =>
        //                {
        //                    options.Filters.Add(new FakeUserFilter());
        //                });
        //            }
        //        });
        //    });
        //    return factory;
        //}
    }
}
