using CarWashShopAPI.DTO.ServiceDTO;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopCreation
    {
        [Required]
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        [Required, Range(1, 1000)]
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        [Required, Range(0, 24)]
        public int OpeningTime { get; set; }
        [Required, Range(0, 24)]
        public int ClosingTime { get; set; }
        public List<ServiceCreationAndUpdate> Services { get; set; }
        public List<string>? CarWashShopsOwners { get; set; } = new List<string>();
    }
}
