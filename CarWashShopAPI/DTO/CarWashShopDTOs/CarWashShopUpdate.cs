using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopUpdate
    {
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        [Required, Range(1,24)]
        public int OpeningTime { get; set; }
        [Required, Range(1, 24)]
        public int ClosingTime { get; set; }
    }
}
