using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public List<ServiceCreationView> Services { get; set; }
    }
}
