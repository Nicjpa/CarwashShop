using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopCreation
    {
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        [Required, Range(0, 23)]
        public int OpeningTime { get; set; }
        [Required, Range(0, 23)]
        public int ClosingTime { get; set; }
        public List<ServiceCreationAndUpdate> Services { get; set; }
        public List<string>? CarWashShopsOwners { get; set; } = new List<string>();
    }
}
