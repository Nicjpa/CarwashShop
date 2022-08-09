namespace CarWashShopAPI.Entities
{
    public class CarWashShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public List<Service> Services { get; set; }
        public List<CarWashShopsServices> CarWashShopsServices { get; set; } = new List<CarWashShopsServices>();    
        public List<CarWashShopsOwners> Owners { get; set; }
    }
}
