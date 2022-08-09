namespace CarWashShopAPI.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<CarWashShopsServices> CarWashShops { get; set; }
    }
}
