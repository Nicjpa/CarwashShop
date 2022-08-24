using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopUpdate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string AdvertisingDescription { get; set; }
        [Required]
        public int AmountOfWashingUnits { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, Range(0,23)]
        public int? OpeningTime { get; set; }
        [Required, Range(0, 23)]
        public int? ClosingTime { get; set; }
    }
}
