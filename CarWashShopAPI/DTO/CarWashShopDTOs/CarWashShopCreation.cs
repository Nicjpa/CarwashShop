using CarWashShopAPI.Entities;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopCreation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public List<Service> Services { get; set; }
        public List<CarWashShopsOwners> CarWashShopsOwners { get; set; }
    }
}
