using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class DisbandRequestCreation
    {
        public string OwnerName { get; set; }
        public string? RequestStatement { get; set; }
    }
}
