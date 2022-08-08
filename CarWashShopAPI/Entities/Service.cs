namespace CarWashShopAPI.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        public int CarWashShopId { get; set; }
        public CarWashShop CarWash { get; set; }
        public decimal Price { get; set; }
    }
}
