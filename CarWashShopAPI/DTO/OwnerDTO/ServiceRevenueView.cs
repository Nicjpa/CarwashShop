using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class ServiceRevenueView
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public int AmountOfBookings { get; set; } = 0;
        public string ServiceRevenue { get { return Revenue.ToString("c"); } }
        [JsonIgnore]
        public decimal Revenue { get; set; } = 0;
    }
}
