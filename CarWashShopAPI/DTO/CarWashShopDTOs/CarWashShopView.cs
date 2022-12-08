using CarWashShopAPI.DTO.ServiceDTO;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        public decimal Revenue { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public bool isInRemovalProcess { get; set; }
        public List<ServiceView> Services { get; set; }
    }
}
