using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class ServiceCreationView
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
