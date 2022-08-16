


using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.Entities
{
    public class CarWashShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        [Required]
        public int OpeningTime { get; set; }
        [Required]
        public int ClosingTime { get; set; }
        public List<CarWashShopsServices> CarWashShopsServices { get; set; } = new List<CarWashShopsServices>();
        public List<CarWashShopsOwners> Owners { get; set; }
    }
}
