using CarWashShopAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI
{
    public class CarWashDbContext : DbContext
    {
        public CarWashDbContext(DbContextOptions<CarWashDbContext> options) : base(options) { }


        public DbSet<CarWashShop> CarWashs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<CarWashShopsOwners> CarWashShopsOwners { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarWashShopsOwners>().HasKey(x => new { x.OwnerId, x.CarWashShopId });

            modelBuilder.Entity<CarWashShopsOwners>()
            .HasOne(cwo => cwo.Owner)
            .WithMany(cwo => cwo.CarWashShopsOwners)
            .HasForeignKey(cwo => cwo.OwnerId);

            modelBuilder.Entity<CarWashShopsOwners>()
            .HasOne(cwo => cwo.CarWashShop)
            .WithMany(cwo => cwo.CarWashShopsOwners)
            .HasForeignKey(cwo => cwo.CarWashShopId);

            //modelBuilder.Entity<Franchise>().HasIndex(u => u.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
