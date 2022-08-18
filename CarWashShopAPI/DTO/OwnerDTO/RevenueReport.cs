namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class RevenueReport
    {
        public int CarWashShopID { get; set; }
        public string CarWashShopName { get; set; }
        public decimal TotalRevenue { get; set; } = 0;
        public List<ServiceRevenue> ByServicesRevenue { get; set; } = new List<ServiceRevenue>();

    }
}
