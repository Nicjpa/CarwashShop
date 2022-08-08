using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.ServiceTypeDTO
{
    public class ServiceTypeCreation
    {
        [Required]
        [MinLength(5), MaxLength(20)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
