
using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class RevenueReportView
    {
        public int CarWashShopID { get; set; }
        public string CarWashShopName { get; set; }
        public int TotalBookings { get; set; } = 0;
        public string TotalRevenue { get { return Revenue.ToString("c"); } }
        public List<ServiceRevenueView> ByServicesRevenue { get; set; } = new List<ServiceRevenueView>();
        [JsonIgnore]
        public decimal Revenue { get; set; } = 0;
    }
}
