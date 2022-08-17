using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class DisbandRequestCreation
    {
        public string CarWashShopName { get; set; }
        public string OwnerName { get; set; }
        public string? RequestStatement { get; set; }
    }
}
