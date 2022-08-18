namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class ServiceRevenue
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public decimal Revenue { get; set; } = 0;
    }
}
