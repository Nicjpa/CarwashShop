using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI
{
    public class CarWashDbContext : IdentityDbContext
    {
        public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options) { }


        public DbSet<CarWashShop> CarWashs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CarWashShopsOwners> CarWashShopsOwners { get; set; }
        

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
