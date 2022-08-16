

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.Entities
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<CarWashShopsServices> CarWashShops { get; set; }
    }
}
