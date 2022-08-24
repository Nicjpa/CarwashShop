namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class ServiceViewWithShopAssigned : ServiceView
    {
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
    }
}
