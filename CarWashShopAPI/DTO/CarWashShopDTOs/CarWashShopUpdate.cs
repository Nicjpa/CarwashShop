using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string AdvertisingDescription { get; set; }
        [Required]
        public int AmountOfWashingUnits { get; set; }
        [Required]
        public string Address { get; set; }
        [Required, Range(0,24)]
        public int? OpeningTime { get; set; }
        [Required, Range(0, 24)]
        public int? ClosingTime { get; set; }
    }
}
