using Newtonsoft.Json;

namespace CarWashShopAPI.Entities
{
    public class CarWashShopsServices
    {
        public int CarWashShopId { get; set; }
        public CarWashShop CarWashShop { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
