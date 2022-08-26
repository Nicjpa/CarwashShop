using CarWashShopAPI.Repositories;
using CarWashShopAPI.Repositories.IRepositories;

namespace CarWashShopAPI.Helpers
{
    public static class DIAdapter
    {
        public static void AddMyServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICarWashRepository, CarWashRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IServiceRepository, ShopServiceRepository>();
            services.AddScoped<IConsumerRepository, ConsumerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddTransient<IRadarRepository, RadarRepository>();
        }
    }
}
