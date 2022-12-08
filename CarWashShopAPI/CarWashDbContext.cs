using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI
{
    public class CarWashDbContext : IdentityDbContext
    {
        public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options) { }

        public DbSet<CarWashShop> CarWashsShops { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CarWashShopsOwners> CarWashShopsOwners { get; set; }
        public DbSet<CarWashShopsServices> CarWashShopsServices { get; set; }
        public DbSet<DisbandRequest> OwnerRemovalRequests { get; set; }
        public DbSet<ShopRemovalRequest> ShopRemovalRequests { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<ShopIncome> Income { get; set; }
      


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarWashShopsOwners>().HasKey(x => new { x.OwnerId, x.CarWashShopId });

            modelBuilder.Entity<CarWashShopsOwners>()
            .HasOne(cwo => cwo.Owner)
            .WithMany(cwo => cwo.CarWashShops)
            .HasForeignKey(cwo => cwo.OwnerId);

            modelBuilder.Entity<CarWashShopsOwners>()
            .HasOne(cwo => cwo.CarWashShop)
            .WithMany(cwo => cwo.Owners)
            .HasForeignKey(cwo => cwo.CarWashShopId);

            modelBuilder.Entity<CarWashShopsServices>().HasKey(x => new { x.ServiceId, x.CarWashShopId });

            modelBuilder.Entity<CarWashShopsServices>()
            .HasOne(cwo => cwo.Service)
            .WithMany(cwo => cwo.CarWashShops)
            .HasForeignKey(cwo => cwo.ServiceId);

            modelBuilder.Entity<CarWashShopsServices>()
            .HasOne(cwo => cwo.CarWashShop)
            .WithMany(cwo => cwo.CarWashShopsServices)
            .HasForeignKey(cwo => cwo.CarWashShopId);

            modelBuilder.Entity<CarWashShop>().HasIndex(u => u.Name).IsUnique();

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<CustomUser>();
            var password = "ValueShore3!";


            // A D M I N I S T R A T O R

            var admin = new CustomUser()
            {
                Id = "24ab6a6c-14f1-4b49-8964-ecfcbce372a3",
                FirstName = "John",
                LastName = "Smith",
                Address = "9th Street",
                Role = "Admin",
                UserName = "jsmith",
                NormalizedUserName = "JSMITH",
                Email = "john.smith@gmail.com",
                NormalizedEmail = "JOHN.SMITH@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 582-282-2749"
            };


            // O W N E R S

            var owner1 = new CustomUser()
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
            };

            var owner2 = new CustomUser()
            {
                Id = "1741abab-ff61-4598-a947-9c798c4ff866",
                FirstName = "Vese",
                LastName = "Calin",
                Address = "Pinewood Heights 70",
                Role = "Owner",
                UserName = "vesec",
                NormalizedUserName = "VESEC",
                Email = "vese.calin@gmail.com",
                NormalizedEmail = "VESE.CALIN@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 312-251-6119"
            };

            var owner3 = new CustomUser()
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
            };

            var owner4 = new CustomUser()
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
            };

            var owner5 = new CustomUser()
            {
                Id = "94084a54-4f4e-4e86-805c-0ba0abdb1ec6",
                FirstName = "Ehab",
                LastName = "Eshaak",
                Address = "Green Street 99",
                Role = "Owner",
                UserName = "ehabes",
                NormalizedUserName = "EHABES",
                Email = "ehab.eshaak@gmail.com",
                NormalizedEmail = "EHAB.ESHAAK@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 315-919-1406"
            };

            var owner6 = new CustomUser()
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
            };

            var owner7 = new CustomUser()
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
            };

            var owner8 = new CustomUser()
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
            };

            var owner9 = new CustomUser()
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
            };

            var owner10 = new CustomUser()
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
            };



            // C O N S U M E R S

            var consumer1 = new CustomUser()
            {
                Id = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                FirstName = "Carlos",
                LastName = "Benavidez",
                Address = "Blackpot Square 12",
                Role = "Consumer",
                UserName = "carlos",
                NormalizedUserName = "CARLOS",
                Email = "carlos.benavidez@gmail.com",
                NormalizedEmail = "CARLOS.BENAVIDEZ@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 215-293-3691"
            };

            var consumer2 = new CustomUser()
            {
                Id = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                FirstName = "Wadzanai",
                LastName = "Bote",
                Address = "Redcliff View 78",
                Role = "Consumer",
                UserName = "wadza",
                NormalizedUserName = "WADZA",
                Email = "wadzanai.bote@gmail.com",
                NormalizedEmail = "WADZANAI.BOTE@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 582-444-7776"
            };

            var consumer3 = new CustomUser()
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
            };

            var consumer4 = new CustomUser()
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
            };

            var consumer5 = new CustomUser()
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
            };

            var consumer6 = new CustomUser()
            {
                Id = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                FirstName = "Martina",
                LastName = "Salerno",
                Address = "Hell's Kitchen 6A",
                Role = "Consumer",
                UserName = "martina",
                NormalizedUserName = "MARTINA",
                Email = "martina.salerno@gmail.com",
                NormalizedEmail = "MARTINA.SALERNO@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 505-644-9019"
            };

            var consumer7 = new CustomUser()
            {
                Id = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                FirstName = "Johnatan",
                LastName = "Garcia",
                Address = "Los Olivos 112",
                Role = "Consumer",
                UserName = "johnatan",
                NormalizedUserName = "JOHNATAN",
                Email = "johnatan.garcia@gmail.com",
                NormalizedEmail = "JOHNATAN.GARCIA@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 218-955-6366"
            };

            var consumer8 = new CustomUser()
            {
                Id = "36838a09-6809-4423-964e-154dea2e45c0",
                FirstName = "Viktor",
                LastName = "Popov",
                Address = "Absolute Street 129",
                Role = "Consumer",
                UserName = "viktor",
                NormalizedUserName = "VIKTOR",
                Email = "viktor.popov@gmail.com",
                NormalizedEmail = "VIKTOR.POPOV@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 218-955-6366"
            };

            var consumer9 = new CustomUser()
            {
                Id = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                FirstName = "Aurimas",
                LastName = "Trunchinskas",
                Address = "66th Street",
                Role = "Consumer",
                UserName = "aurit",
                NormalizedUserName = "AURIT",
                Email = "aurimas.trunchinskas@gmail.com",
                NormalizedEmail = "AURIMAS.TRUNCHINSKAS@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 304-894-4852"
            };

            var consumer10 = new CustomUser()
            {
                Id = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                FirstName = "Robert",
                LastName = "Bradford",
                Address = "Whiteroof Valley",
                Role = "Consumer",
                UserName = "robber",
                NormalizedUserName = "ROBBER",
                Email = "robert.bradford@gmail.com",
                NormalizedEmail = "ROBERT.BRADFORD@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, password),
                PhoneNumber = "+1 304-894-4852"
            };



            // CAR WASH  S H O P

            var shop1 = new CarWashShop()
            {
                Id = 1,
                Name = "Waterloo",
                AdvertisingDescription = "Fast, Clean and Waterloo",
                AmountOfWashingUnits = 10,
                Address = "Sunshine road 99",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop2 = new CarWashShop()
            {
                Id = 2,
                Name = "Geyser Blaze",
                AdvertisingDescription = "Thorough and professional cleaning",
                AmountOfWashingUnits = 8,
                Address = "Black desert street 75",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop3 = new CarWashShop()
            {
                Id = 3,
                Name = "Vehicle Washing Center",
                AdvertisingDescription = "Biggest in the city",
                AmountOfWashingUnits = 25,
                Address = "Mellwood Pine 44",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop4 = new CarWashShop()
            {
                Id = 4,
                Name = "Real Wash",
                AdvertisingDescription = "Pure and clean",
                AmountOfWashingUnits = 6,
                Address = "Rocky Mountain 56",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop5 = new CarWashShop()
            {
                Id = 5,
                Name = "Phantom Wash",
                AdvertisingDescription = "Super fast and furious",
                AmountOfWashingUnits = 12,
                Address = "Main Square 96",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop6 = new CarWashShop()
            {
                Id = 6,
                Name = "BubbleTime",
                AdvertisingDescription = "Refresh your vehicle",
                AmountOfWashingUnits = 5,
                Address = "Pinapple Block 82",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop7 = new CarWashShop()
            {
                Id = 7,
                Name = "Purifying Station",
                AdvertisingDescription = "Best you've seen so far",
                AmountOfWashingUnits = 6,
                Address = "Melon Valley 27",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop8 = new CarWashShop()
            {
                Id = 8,
                Name = "EazyPizzy",
                AdvertisingDescription = "Get you done fast and smooth",
                AmountOfWashingUnits = 8,
                Address = "Dusty Road 33",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop9 = new CarWashShop()
            {
                Id = 9,
                Name = "Emerald Wash",
                AdvertisingDescription = "Your car's appearance matters",
                AmountOfWashingUnits = 10,
                Address = "Sunrise Hill 206",
                OpeningTime = 8,
                ClosingTime = 23
            };

            var shop10 = new CarWashShop()
            {
                Id = 10,
                Name = "Tsunami Wash",
                AdvertisingDescription = "Can't ignore the quality",
                AmountOfWashingUnits = 25,
                Address = "Riverside Downstreet 66",
                OpeningTime = 8,
                ClosingTime = 23
            };



            // CAR WASH  S E R V I C E S

            var service1 = new Service()
            {
                Id = 1,
                Name = "Tsunami STANDARD",
                Description = "Basic outside cleaning",
                Price = 10.75M
            };

            var service2 = new Service()
            {
                Id = 2,
                Name = "Tsunami EXTENDED",
                Description = "STANDARD + extra polishing",
                Price = 14.99M
            };

            var service3 = new Service()
            {
                Id = 3,
                Name = "Tsunami GRAND",
                Description = "EXTENDED + inside deep cleaning",
                Price = 18.25M
            };

            var service4 = new Service()
            {
                Id = 4,
                Name = "Tsunami PREMIUM",
                Description = "GRAND + free beer and wifi while you wait",
                Price = 23.50M
            };

            var service5 = new Service()
            {
                Id = 5,
                Name = "Emerald BRIGHT",
                Description = "desc",
                Price = 12.50M
            };

            var service6 = new Service()
            {
                Id = 6,
                Name = "Emerald SHINE",
                Description = "desc",
                Price = 16.75M
            };

            var service7 = new Service()
            {
                Id = 7,
                Name = "Emerald DIVINE",
                Description = "desc",
                Price = 24.99M
            };

            var service8 = new Service()
            {
                Id = 8,
                Name = "EazyPizzy FASTACTION",
                Description = "desc",
                Price = 10M
            };

            var service9 = new Service()
            {
                Id = 9,
                Name = "EazyPizzy STANDARD",
                Description = "desc",
                Price = 14.20M
            };

            var service10 = new Service()
            {
                Id = 10,
                Name = "EazyPizzy SPECIALACTION",
                Description = "desc",
                Price = 19.25M
            };

            var service11 = new Service()
            {
                Id = 11,
                Name = "Purifying BASIC",
                Description = "desc",
                Price = 12M
            };

            var service12 = new Service()
            {
                Id = 12,
                Name = "Purifying STANDARD",
                Description = "desc",
                Price = 15.75M
            };

            var service13 = new Service()
            {
                Id = 13,
                Name = "Purifying FULL",
                Description = "desc",
                Price = 20M
            };

            var service14 = new Service()
            {
                Id = 14,
                Name = "Bubble CASUAL",
                Description = "desc",
                Price = 7.99M
            };

            var service15 = new Service()
            {
                Id = 15,
                Name = "Bubble EXTRA",
                Description = "desc",
                Price = 12M
            };

            var service16 = new Service()
            {
                Id = 16,
                Name = "Bubble FLOOD",
                Description = "desc",
                Price = 16.25M
            };

            var service17 = new Service()
            {
                Id = 17,
                Name = "Phantom CLASS",
                Description = "desc",
                Price = 12.75M
            };

            var service18 = new Service()
            {
                Id = 18,
                Name = "Phantom OUTSTANDING",
                Description = "desc",
                Price = 16.50M
            };

            var service19 = new Service()
            {
                Id = 19,
                Name = "Phantom LEGENDARY",
                Description = "desc",
                Price = 22.90M
            };

            var service20 = new Service()
            {
                Id = 20,
                Name = "RealWash CLEAN PACK",
                Description = "desc",
                Price = 12M
            };

            var service21 = new Service()
            {
                Id = 21,
                Name = "RealWash BRIGHT PACK",
                Description = "desc",
                Price = 14.75M
            };

            var service22 = new Service()
            {
                Id = 22,
                Name = "RealWash GODLIKE SHINE PACK",
                Description = "desc",
                Price = 18.60M
            };

            var service23 = new Service()
            {
                Id = 23,
                Name = "Vehicle LIGHT",
                Description = "desc",
                Price = 11.50M
            };

            var service24 = new Service()
            {
                Id = 24,
                Name = "Vehicle STANDARD",
                Description = "desc",
                Price = 15.50M
            };

            var service25 = new Service()
            {
                Id = 25,
                Name = "Vehicle MARVELOUS",
                Description = "desc",
                Price = 19.99M
            };

            var service26 = new Service()
            {
                Id = 26,
                Name = "Geyser LIGHT",
                Description = "desc",
                Price = 10.70M
            };

            var service27 = new Service()
            {
                Id = 27,
                Name = "Geyser MEDIUM",
                Description = "desc",
                Price = 15.25M
            };

            var service28 = new Service()
            {
                Id = 28,
                Name = "Geyser BLAZE",
                Description = "desc",
                Price = 18.75M
            };

            var service29 = new Service()
            {
                Id = 29,
                Name = "Waterloo STANDARD",
                Description = "desc",
                Price = 12.20M
            };

            var service30 = new Service()
            {
                Id = 30,
                Name = "Waterloo PREMIUM",
                Description = "desc",
                Price = 16.99M
            };


            // MANY TO MANY "SHOP-SERVICE"

            var shop_service1  = new CarWashShopsServices() { CarWashShopId = 10, ServiceId = 1 };
            var shop_service2  = new CarWashShopsServices() { CarWashShopId = 10, ServiceId = 2 };
            var shop_service3  = new CarWashShopsServices() { CarWashShopId = 10, ServiceId = 3 };
            var shop_service4  = new CarWashShopsServices() { CarWashShopId = 10, ServiceId = 4 };
            var shop_service5  = new CarWashShopsServices() { CarWashShopId = 9, ServiceId = 5 };
            var shop_service6  = new CarWashShopsServices() { CarWashShopId = 9, ServiceId = 6 };
            var shop_service7  = new CarWashShopsServices() { CarWashShopId = 9, ServiceId = 7 };
            var shop_service8  = new CarWashShopsServices() { CarWashShopId = 8, ServiceId = 8 };
            var shop_service9  = new CarWashShopsServices() { CarWashShopId = 8, ServiceId = 9 };
            var shop_service10 = new CarWashShopsServices() { CarWashShopId = 8, ServiceId = 10 };
            var shop_service11 = new CarWashShopsServices() { CarWashShopId = 7, ServiceId = 11 };
            var shop_service12 = new CarWashShopsServices() { CarWashShopId = 7, ServiceId = 12 };
            var shop_service13 = new CarWashShopsServices() { CarWashShopId = 7, ServiceId = 13 };
            var shop_service14 = new CarWashShopsServices() { CarWashShopId = 6, ServiceId = 14 };
            var shop_service15 = new CarWashShopsServices() { CarWashShopId = 6, ServiceId = 15 };
            var shop_service16 = new CarWashShopsServices() { CarWashShopId = 6, ServiceId = 16 };
            var shop_service17 = new CarWashShopsServices() { CarWashShopId = 5, ServiceId = 17 };
            var shop_service18 = new CarWashShopsServices() { CarWashShopId = 5, ServiceId = 18 };
            var shop_service19 = new CarWashShopsServices() { CarWashShopId = 5, ServiceId = 19 };
            var shop_service20 = new CarWashShopsServices() { CarWashShopId = 4, ServiceId = 20 };
            var shop_service21 = new CarWashShopsServices() { CarWashShopId = 4, ServiceId = 21 };
            var shop_service22 = new CarWashShopsServices() { CarWashShopId = 4, ServiceId = 22 };
            var shop_service23 = new CarWashShopsServices() { CarWashShopId = 3, ServiceId = 23 };
            var shop_service24 = new CarWashShopsServices() { CarWashShopId = 3, ServiceId = 24 };
            var shop_service25 = new CarWashShopsServices() { CarWashShopId = 3, ServiceId = 25 };
            var shop_service26 = new CarWashShopsServices() { CarWashShopId = 2, ServiceId = 26 };
            var shop_service27 = new CarWashShopsServices() { CarWashShopId = 2, ServiceId = 27 };
            var shop_service28 = new CarWashShopsServices() { CarWashShopId = 2, ServiceId = 28 };
            var shop_service29 = new CarWashShopsServices() { CarWashShopId = 1, ServiceId = 29 };
            var shop_service30 = new CarWashShopsServices() { CarWashShopId = 1, ServiceId = 30 };

            // MANY TO MANY "SHOP-OWNERS"

            var shop_owner1 =  new CarWashShopsOwners() { CarWashShopId = 1, OwnerId = "6f57119a-6b89-43ed-8df4-4b70d5259548" };
            var shop_owner2 =  new CarWashShopsOwners() { CarWashShopId = 2, OwnerId = "6f57119a-6b89-43ed-8df4-4b70d5259548" };
            var shop_owner3 =  new CarWashShopsOwners() { CarWashShopId = 2, OwnerId = "1741abab-ff61-4598-a947-9c798c4ff866" };
            var shop_owner4 =  new CarWashShopsOwners() { CarWashShopId = 3, OwnerId = "f4352621-5ced-4afa-854f-49a10819d206" };
            var shop_owner5 =  new CarWashShopsOwners() { CarWashShopId = 3, OwnerId = "74ea7ef1-0444-447a-9780-0b3a0126a20b" };
            var shop_owner6 =  new CarWashShopsOwners() { CarWashShopId = 3, OwnerId = "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" };
            var shop_owner7 =  new CarWashShopsOwners() { CarWashShopId = 4, OwnerId = "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" };
            var shop_owner8 =  new CarWashShopsOwners() { CarWashShopId = 5, OwnerId = "e8952694-1ca9-44b1-a8fa-73988bb4eee5" };
            var shop_owner9 =  new CarWashShopsOwners() { CarWashShopId = 6, OwnerId = "94084a54-4f4e-4e86-805c-0ba0abdb1ec6" };
            var shop_owner10 = new CarWashShopsOwners() { CarWashShopId = 6, OwnerId = "edebb245-2066-4126-b9e4-dc020ffdafe7" };
            var shop_owner11 = new CarWashShopsOwners() { CarWashShopId = 7, OwnerId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f" };
            var shop_owner12 = new CarWashShopsOwners() { CarWashShopId = 8, OwnerId = "f02b000c-622d-4c3f-b215-7e08cea2469c" };
            var shop_owner13 = new CarWashShopsOwners() { CarWashShopId = 9, OwnerId = "f02b000c-622d-4c3f-b215-7e08cea2469c" };
            var shop_owner14 = new CarWashShopsOwners() { CarWashShopId = 10, OwnerId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f" };
            var shop_owner15 = new CarWashShopsOwners() { CarWashShopId = 10, OwnerId = "71a07f92-c8b6-47a8-8f1f-0eb340062e57" };
            var shop_owner16 = new CarWashShopsOwners() { CarWashShopId = 10, OwnerId = "6f57119a-6b89-43ed-8df4-4b70d5259548" };


            // B O O K I N G S

            var booking1 = new Booking()
            {
                Id = 1,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 1,
                ServiceId = 30,
                ScheduledDateTime = new DateTime(2022, 1, 10, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.99M
            };

            var booking2 = new Booking()
            {
                Id = 2,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 1,
                ServiceId = 29,
                ScheduledDateTime = new DateTime(2022, 1, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.20M
            };

            var booking3= new Booking()
            {
                Id = 3,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 1,
                ServiceId = 30,
                ScheduledDateTime = new DateTime(2022, 1, 14, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.99M
            };

            var booking4 = new Booking()
            {
                Id = 4,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 1,
                ServiceId = 30,
                ScheduledDateTime = new DateTime(2022, 1, 18, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.99M
            };

            var booking5 = new Booking()
            {
                Id = 5,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 1,
                ServiceId = 29,
                ScheduledDateTime = new DateTime(2022, 1, 22, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.20M
            };

            var booking6 = new Booking()
            {
                Id = 6,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 2,
                ServiceId = 28,
                ScheduledDateTime = new DateTime(2022, 1, 26, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.75M
            };

            var booking7 = new Booking()
            {
                Id = 7,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 2,
                ServiceId = 26,
                ScheduledDateTime = new DateTime(2022, 1, 27, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.70M
            };

            var booking8 = new Booking()
            {
                Id = 8,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 2,
                ServiceId = 27,
                ScheduledDateTime = new DateTime(2022, 2, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.25M
            };

            var booking9 = new Booking()
            {
                Id = 9,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 3,
                ServiceId = 25,
                ScheduledDateTime = new DateTime(2022, 2, 8, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.99M
            };

            var booking10 = new Booking()
            {
                Id = 10,
                ConsumerId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                CarWashShopId = 3,
                ServiceId = 24,
                ScheduledDateTime = new DateTime(2022, 2, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.50M
            };



            var booking11 = new Booking()
            {
                Id = 11,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 2,
                ServiceId = 28,
                ScheduledDateTime = new DateTime(2022, 1, 25, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.75M
            };

            var booking12 = new Booking()
            {
                Id = 12,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 2,
                ServiceId = 26,
                ScheduledDateTime = new DateTime(2022, 2, 1, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.70M
            };

            var booking13 = new Booking()
            {
                Id = 13,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 2,
                ServiceId = 28,
                ScheduledDateTime = new DateTime(2022, 2, 6, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.75M
            };

            var booking14 = new Booking()
            {
                Id = 14,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 2,
                ServiceId = 27,
                ScheduledDateTime = new DateTime(2022, 2, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.25M
            };

            var booking15 = new Booking()
            {
                Id = 15,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 25,
                ScheduledDateTime = new DateTime(2022, 2, 16, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.99M
            };

            var booking16 = new Booking()
            {
                Id = 16,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 24,
                ScheduledDateTime = new DateTime(2022, 2, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.50M
            };

            var booking17 = new Booking()
            {
                Id = 17,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 23,
                ScheduledDateTime = new DateTime(2022, 2, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 11.50M
            };

            var booking18 = new Booking()
            {
                Id = 18,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 25,
                ScheduledDateTime = new DateTime(2022, 2, 27, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.99M
            };

            var booking19 = new Booking()
            {
                Id = 19,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 24,
                ScheduledDateTime = new DateTime(2022, 2, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.50M
            };

            var booking20 = new Booking()
            {
                Id = 20,
                ConsumerId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                CarWashShopId = 3,
                ServiceId = 23,
                ScheduledDateTime = new DateTime(2022, 3, 5, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 11.50M
            };



            var booking21 = new Booking()
            {
                Id = 21,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 3,
                ServiceId = 25,
                ScheduledDateTime = new DateTime(2022, 2, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.99M
            };

            var booking22 = new Booking()
            {
                Id = 22,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 3,
                ServiceId = 24,
                ScheduledDateTime = new DateTime(2022, 2, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.50M
            };

            var booking23 = new Booking()
            {
                Id = 23,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 3,
                ServiceId = 23,
                ScheduledDateTime = new DateTime(2022, 2, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 11.50M
            };

            var booking24 = new Booking()
            {
                Id = 24,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 4,
                ServiceId = 22,
                ScheduledDateTime = new DateTime(2022, 3, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.60M
            };

            var booking25 = new Booking()
            {
                Id = 25,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 4,
                ServiceId = 21,
                ScheduledDateTime = new DateTime(2022, 3, 9, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.75M
            };

            var booking26 = new Booking()
            {
                Id = 26,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 4,
                ServiceId = 20,
                ScheduledDateTime = new DateTime(2022, 3, 14, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking27 = new Booking()
            {
                Id = 27,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 4,
                ServiceId = 22,
                ScheduledDateTime = new DateTime(2022, 3, 18, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.60M
            };

            var booking28 = new Booking()
            {
                Id = 28,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 4,
                ServiceId = 21,
                ScheduledDateTime = new DateTime(2022, 3, 22, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.75M
            };

            var booking29 = new Booking()
            {
                Id = 29,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 5,
                ServiceId = 19,
                ScheduledDateTime = new DateTime(2022, 3, 26, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 22.90M
            };

            var booking30 = new Booking()
            {
                Id = 30,
                ConsumerId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                CarWashShopId = 5,
                ServiceId = 18,
                ScheduledDateTime = new DateTime(2022, 3, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.50M
            };


            var booking31 = new Booking()
            {
                Id = 31,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 4,
                ServiceId = 22,
                ScheduledDateTime = new DateTime(2022, 3, 15, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.60M
            };

            var booking32 = new Booking()
            {
                Id = 32,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 4,
                ServiceId = 21,
                ScheduledDateTime = new DateTime(2022, 3, 19, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.75M
            };

            var booking33 = new Booking()
            {
                Id = 33,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 4,
                ServiceId = 22,
                ScheduledDateTime = new DateTime(2022, 3, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.60M
            };

            var booking34 = new Booking()
            {
                Id = 34,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 4,
                ServiceId = 20,
                ScheduledDateTime = new DateTime(2022, 3, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking35 = new Booking()
            {
                Id = 35,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 5,
                ServiceId = 19,
                ScheduledDateTime = new DateTime(2022, 4, 1, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 22.90M
            };

            var booking36 = new Booking()
            {
                Id = 36,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 5,
                ServiceId = 18,
                ScheduledDateTime = new DateTime(2022, 4, 6, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.50M
            };

            var booking37 = new Booking()
            {
                Id = 37,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 5,
                ServiceId = 17,
                ScheduledDateTime = new DateTime(2022, 4, 10, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.75M
            };

            var booking38 = new Booking()
            {
                Id = 38,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 5,
                ServiceId = 18,
                ScheduledDateTime = new DateTime(2022, 4, 16, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.50M
            };

            var booking39 = new Booking()
            {
                Id = 39,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 4, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };

            var booking40 = new Booking()
            {
                Id = 40,
                ConsumerId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 4, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };



            var booking41 = new Booking()
            {
                Id = 41,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 5,
                ServiceId = 19,
                ScheduledDateTime = new DateTime(2022, 3, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 22.90M
            };

            var booking42 = new Booking()
            {
                Id = 42,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 5,
                ServiceId = 18,
                ScheduledDateTime = new DateTime(2022, 4, 8, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.50M
            };

            var booking43 = new Booking()
            {
                Id = 43,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 5,
                ServiceId = 17,
                ScheduledDateTime = new DateTime(2022, 4, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.75M
            };

            var booking44 = new Booking()
            {
                Id = 44,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 5,
                ServiceId = 17,
                ScheduledDateTime = new DateTime(2022, 4, 18, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.75M
            };

            var booking45 = new Booking()
            {
                Id = 45,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 4, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };

            var booking46 = new Booking()
            {
                Id = 46,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 4, 29, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking47 = new Booking()
            {
                Id = 47,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 14,
                ScheduledDateTime = new DateTime(2022, 5, 5, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 7.99M
            };

            var booking48 = new Booking()
            {
                Id = 48,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 14,
                ScheduledDateTime = new DateTime(2022, 5, 16, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 7.99M
            };

            var booking49 = new Booking()
            {
                Id = 49,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 5, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking50 = new Booking()
            {
                Id = 50,
                ConsumerId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 5, 25, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };



            var booking51 = new Booking()
            {
                Id = 51,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 5,
                ServiceId = 19,
                ScheduledDateTime = new DateTime(2022, 5, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 22.90M
            };

            var booking52 = new Booking()
            {
                Id = 52,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 5,
                ServiceId = 18,
                ScheduledDateTime = new DateTime(2022, 5, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.50M
            };

            var booking53 = new Booking()
            {
                Id = 53,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 6, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };

            var booking54 = new Booking()
            {
                Id = 54,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 6, 10, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking55 = new Booking()
            {
                Id = 55,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 6,
                ServiceId = 14,
                ScheduledDateTime = new DateTime(2022, 6, 14, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 7.99M
            };

            var booking56 = new Booking()
            {
                Id = 56,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 6, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking57 = new Booking()
            {
                Id = 57,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 6,
                ServiceId = 14,
                ScheduledDateTime = new DateTime(2022, 6, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 7.99M
            };

            var booking58 = new Booking()
            {
                Id = 58,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 7,
                ServiceId = 13,
                ScheduledDateTime = new DateTime(2022, 6, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 20.00M
            };

            var booking59 = new Booking()
            {
                Id = 59,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 7,
                ServiceId = 12,
                ScheduledDateTime = new DateTime(2022, 7, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.75M
            };

            var booking60 = new Booking()
            {
                Id = 60,
                ConsumerId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                CarWashShopId = 7,
                ServiceId = 11,
                ScheduledDateTime = new DateTime(2022, 7, 10, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 5.00M
            };



            var booking61 = new Booking()
            {
                Id = 61,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 6,
                ServiceId = 14,
                ScheduledDateTime = new DateTime(2022, 6, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 7.99M
            };

            var booking62 = new Booking()
            {
                Id = 62,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 6,
                ServiceId = 15,
                ScheduledDateTime = new DateTime(2022, 6, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.00M
            };

            var booking63 = new Booking()
            {
                Id = 63,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 7, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };

            var booking64 = new Booking()
            {
                Id = 64,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 6,
                ServiceId = 16,
                ScheduledDateTime = new DateTime(2022, 7, 8, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.25M
            };

            var booking65 = new Booking()
            {
                Id = 65,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 13,
                ScheduledDateTime = new DateTime(2022, 7, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 20.00M
            };

            var booking66 = new Booking()
            {
                Id = 66,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 12,
                ScheduledDateTime = new DateTime(2022, 7, 16, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.75M
            };

            var booking67 = new Booking()
            {
                Id = 67,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 11,
                ScheduledDateTime = new DateTime(2022, 7, 22, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 5.00M
            };

            var booking68 = new Booking()
            {
                Id = 68,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 11,
                ScheduledDateTime = new DateTime(2022, 7, 26, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 5.00M
            };

            var booking69 = new Booking()
            {
                Id = 69,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 12,
                ScheduledDateTime = new DateTime(2022, 7, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.75M
            };

            var booking70 = new Booking()
            {
                Id = 70,
                ConsumerId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                CarWashShopId = 7,
                ServiceId = 13,
                ScheduledDateTime = new DateTime(2022, 8, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 20.00M
            };



            var booking71 = new Booking()
            {
                Id = 71,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 7,
                ServiceId = 13,
                ScheduledDateTime = new DateTime(2022, 6, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 20.00M
            };

            var booking72 = new Booking()
            {
                Id = 72,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 7,
                ServiceId = 12,
                ScheduledDateTime = new DateTime(2022, 6, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 15.75M
            };

            var booking73 = new Booking()
            {
                Id = 73,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 7,
                ServiceId = 11,
                ScheduledDateTime = new DateTime(2022, 7, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 5.00M
            };

            var booking74 = new Booking()
            {
                Id = 74,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 7,
                ServiceId = 11,
                ScheduledDateTime = new DateTime(2022, 7, 8, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 5.00M
            };

            var booking75 = new Booking()
            {
                Id = 75,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 10,
                ScheduledDateTime = new DateTime(2022, 7, 12, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.25M
            };

            var booking76 = new Booking()
            {
                Id = 76,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 9,
                ScheduledDateTime = new DateTime(2022, 7, 16, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.20M
            };

            var booking77 = new Booking()
            {
                Id = 77,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 8,
                ScheduledDateTime = new DateTime(2022, 7, 22, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.00M
            };

            var booking78 = new Booking()
            {
                Id = 78,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 8,
                ScheduledDateTime = new DateTime(2022, 7, 26, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.00M
            };

            var booking79 = new Booking()
            {
                Id = 79,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 9,
                ScheduledDateTime = new DateTime(2022, 7, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.20M
            };

            var booking80 = new Booking()
            {
                Id = 80,
                ConsumerId = "36838a09-6809-4423-964e-154dea2e45c0",
                CarWashShopId = 8,
                ServiceId = 10,
                ScheduledDateTime = new DateTime(2022, 8, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.25M
            };



            var booking81 = new Booking()
            {
                Id = 81,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 8,
                ServiceId = 9,
                ScheduledDateTime = new DateTime(2022, 6, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.20M
            };

            var booking82 = new Booking()
            {
                Id = 82,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 8,
                ServiceId = 10,
                ScheduledDateTime = new DateTime(2022, 6, 28, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 19.25M
            };

            var booking83 = new Booking()
            {
                Id = 83,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 8,
                ServiceId = 8,
                ScheduledDateTime = new DateTime(2022, 7, 6, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.00M
            };

            var booking84 = new Booking()
            {
                Id = 84,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 8,
                ServiceId = 9,
                ScheduledDateTime = new DateTime(2022, 7, 10, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.20M
            };

            var booking85 = new Booking()
            {
                Id = 85,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 7,
                ScheduledDateTime = new DateTime(2022, 7, 15, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 24.99M
            };

            var booking86 = new Booking()
            {
                Id = 86,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 6,
                ScheduledDateTime = new DateTime(2022, 7, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.75M
            };

            var booking87 = new Booking()
            {
                Id = 87,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 5,
                ScheduledDateTime = new DateTime(2022, 7, 24, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.50M
            };

            var booking88 = new Booking()
            {
                Id = 88,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 5,
                ScheduledDateTime = new DateTime(2022, 7, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 12.50M
            };

            var booking89 = new Booking()
            {
                Id = 89,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 6,
                ScheduledDateTime = new DateTime(2022, 8, 4, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.75M
            };

            var booking90 = new Booking()
            {
                Id = 90,
                ConsumerId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                CarWashShopId = 9,
                ServiceId = 7,
                ScheduledDateTime = new DateTime(2022, 8, 9, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 24.99M
            };



            var booking91 = new Booking()
            {
                Id = 91,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 9,
                ServiceId = 7,
                ScheduledDateTime = new DateTime(2022, 7, 20, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 24.99M
            };

            var booking92 = new Booking()
            {
                Id = 92,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 9,
                ServiceId = 6,
                ScheduledDateTime = new DateTime(2022, 7, 26, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 16.75M
            };

            var booking93 = new Booking()
            {
                Id = 93,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 1,
                ScheduledDateTime = new DateTime(2022, 7, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.75M
            };

            var booking94 = new Booking()
            {
                Id = 94,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 2,
                ScheduledDateTime = new DateTime(2022, 8, 5, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.99M
            };

            var booking95 = new Booking()
            {
                Id = 95,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 3,
                ScheduledDateTime = new DateTime(2022, 8, 9, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.25M
            };

            var booking96 = new Booking()
            {
                Id = 96,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 4,
                ScheduledDateTime = new DateTime(2022, 8, 14, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 23.50M
            };

            var booking97 = new Booking()
            {
                Id = 97,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 4,
                ScheduledDateTime = new DateTime(2022, 8, 18, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 23.50M
            };

            var booking98 = new Booking()
            {
                Id = 98,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 3,
                ScheduledDateTime = new DateTime(2022, 8, 21, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 18.25M
            };

            var booking99 = new Booking()
            {
                Id = 99,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 2,
                ScheduledDateTime = new DateTime(2022, 8, 25, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 14.99M
            };

            var booking100 = new Booking()
            {
                Id = 100,
                ConsumerId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                CarWashShopId = 10,
                ServiceId = 1,
                ScheduledDateTime = new DateTime(2022, 8, 30, 10, 0, 0),
                IsPaid = false,
                BookingStatus = BookingStatus.Confirmed,
                DateCreated = new DateTime(2022, 1, 5, 10, 0, 0),
                Price = 10.75M
            };


            // ADMIN CLAIM
            var adminClaim = new IdentityUserClaim<string>()
            {
                Id = 1,
                UserId = "24ab6a6c-14f1-4b49-8964-ecfcbce372a3",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Admin"
            };

            // OWNER CLAIMS
            var ownerClaim1 = new IdentityUserClaim<string>()
            {
                Id = 2,
                UserId = "6f57119a-6b89-43ed-8df4-4b70d5259548",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim2 = new IdentityUserClaim<string>()
            {
                Id = 3,
                UserId = "1741abab-ff61-4598-a947-9c798c4ff866",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim3 = new IdentityUserClaim<string>()
            {
                Id = 4,
                UserId = "f4352621-5ced-4afa-854f-49a10819d206",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim4 = new IdentityUserClaim<string>()
            {
                Id = 5,
                UserId = "74ea7ef1-0444-447a-9780-0b3a0126a20b",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim5 = new IdentityUserClaim<string>()
            {
                Id = 6,
                UserId = "94084a54-4f4e-4e86-805c-0ba0abdb1ec6",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim6 = new IdentityUserClaim<string>()
            {
                Id = 7,
                UserId = "edebb245-2066-4126-b9e4-dc020ffdafe7",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim7 = new IdentityUserClaim<string>()
            {
                Id = 8,
                UserId = "e8952694-1ca9-44b1-a8fa-73988bb4eee5",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim8 = new IdentityUserClaim<string>()
            {
                Id = 9,
                UserId = "caba2ea1-ab92-4db7-a2fa-0d01d6d6195f",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim9 = new IdentityUserClaim<string>()
            {
                Id = 10,
                UserId = "f02b000c-622d-4c3f-b215-7e08cea2469c",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };

            var ownerClaim10 = new IdentityUserClaim<string>()
            {
                Id = 11,
                UserId = "71a07f92-c8b6-47a8-8f1f-0eb340062e57",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Owner"
            };


            // CONSUMER CLAIMS
            var consumerClaim1 = new IdentityUserClaim<string>()
            {
                Id = 12,
                UserId = "b48c3cdd-dc9a-4d9d-af2c-420a68556126",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim2 = new IdentityUserClaim<string>()
            {
                Id = 13,
                UserId = "06638581-8f0c-4119-a637-e4f3b5bbe858",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim3 = new IdentityUserClaim<string>()
            {
                Id = 14,
                UserId = "a73fc0f6-3559-4848-9224-099903fcdca2",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim4 = new IdentityUserClaim<string>()
            {
                Id = 15,
                UserId = "dbf1bf5c-8485-4ebb-9d83-3806149d8048",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim5 = new IdentityUserClaim<string>()
            {
                Id = 16,
                UserId = "989b1e73-da14-4218-ac8c-d60aaf816520",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim6 = new IdentityUserClaim<string>()
            {
                Id = 17,
                UserId = "c4b2e35a-d562-483a-9c89-f4a3d3d59e77",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim7 = new IdentityUserClaim<string>()
            {
                Id = 18,
                UserId = "56c4a3a6-cc46-4c6d-85cd-2d19a25835df",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim8 = new IdentityUserClaim<string>()
            {
                Id = 19,
                UserId = "36838a09-6809-4423-964e-154dea2e45c0",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim9 = new IdentityUserClaim<string>()
            {
                Id = 20,
                UserId = "2e5be4cb-41c8-4265-8959-e6558a272b62",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };

            var consumerClaim10 = new IdentityUserClaim<string>()
            {
                Id = 21,
                UserId = "1e2723ba-bed4-4f4e-a56f-9fd8abd53e7b",
                ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ClaimValue = "Consumer"
            };


            var users = new List<CustomUser>()
            {
                admin,
                owner1, owner2, owner3, owner4, owner5, owner6, owner7, owner8, owner9, owner10,
                consumer1, consumer2, consumer3, consumer4, consumer5, consumer6, consumer7, consumer8, consumer9, consumer10
            };

            var shops = new List<CarWashShop>()
            { 
                shop1, shop2, shop3, shop4, shop5, shop6, shop7, shop8, shop9, shop10
            };

            var services = new List<Service>()
            {
                service1, service2, service3, service4, service5, service6, service7, service8, service9, service10,
                service11, service12, service13, service14, service15, service16, service17, service18, service19, service20,
                service21, service22, service23, service24, service25, service26, service27, service28, service29, service30
            };

            var bookings = new List<Booking>()
            { 
                booking1, booking2, booking3, booking4, booking5, booking6, booking7, booking8, booking9, booking10,
                booking11, booking12, booking13, booking14, booking15, booking16, booking17, booking18, booking19, booking20,
                booking21, booking22, booking23, booking24, booking25, booking26, booking27, booking28, booking29, booking30,
                booking31, booking32, booking33, booking34, booking35, booking36, booking37, booking38, booking39, booking40,
                booking41, booking42, booking43, booking44, booking45, booking46, booking47, booking48, booking49, booking50,
                booking51, booking52, booking53, booking54, booking55, booking56, booking57, booking58, booking59, booking60,
                booking61, booking62, booking63, booking64, booking65, booking66, booking67, booking68, booking69, booking70,
                booking71, booking72, booking73, booking74, booking75, booking76, booking77, booking78, booking79, booking80,
                booking81, booking82, booking83, booking84, booking85, booking86, booking87, booking88, booking89, booking90,
                booking91, booking92, booking93, booking94, booking95, booking96, booking97, booking98, booking99, booking100
            };

            var shops_owners = new List<CarWashShopsOwners>()
            {
                shop_owner1, shop_owner2, shop_owner3, shop_owner4 ,shop_owner5 ,shop_owner6 ,shop_owner7 ,shop_owner8,
                shop_owner9, shop_owner10, shop_owner11, shop_owner12 ,shop_owner13 ,shop_owner14 ,shop_owner15 ,shop_owner16
            };

            var shops_services = new List<CarWashShopsServices>()
            { 
                shop_service1, shop_service2, shop_service3, shop_service4, shop_service5, shop_service6, shop_service7, shop_service8, shop_service9, shop_service10,
                shop_service11, shop_service12, shop_service13, shop_service14, shop_service15, shop_service16, shop_service17, shop_service18, shop_service19, shop_service20,
                shop_service21, shop_service22, shop_service23, shop_service24, shop_service25, shop_service26, shop_service27, shop_service28, shop_service29, shop_service30,
            };

            var claims = new List<IdentityUserClaim<string>>()
            {
                adminClaim,
                ownerClaim1, ownerClaim2, ownerClaim3, ownerClaim4, ownerClaim5, ownerClaim6, ownerClaim7, ownerClaim8, ownerClaim9, ownerClaim10,
                consumerClaim1, consumerClaim2, consumerClaim3, consumerClaim4, consumerClaim5, consumerClaim6, consumerClaim7, consumerClaim8, consumerClaim9, consumerClaim10
            };

            modelBuilder.Entity<CustomUser>().HasData(users);
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(claims);
            modelBuilder.Entity<CarWashShop>().HasData(shops);
            modelBuilder.Entity<CarWashShopsOwners>().HasData(shops_owners);
            modelBuilder.Entity<Service>().HasData(services);
            modelBuilder.Entity<CarWashShopsServices>().HasData(shops_services);
            modelBuilder.Entity<Booking>().HasData(bookings);
        }
    }
}
