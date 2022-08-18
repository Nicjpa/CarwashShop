using System.Text.Json.Serialization;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopRemovalRequestCreation
    {
        public string CarWashShopName { get; set; }
        public string? RequestStatement { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get { return DateTime.Now; } }
    }
}
