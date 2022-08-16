namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnerShopRemovalRequestView
    {
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public string? RequestStatement { get; set; }
        public bool IsApproved { get; set; }
    }
}
