namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnerRequestsFilters
    {
        public int? CarWashShopId { get; set; }
        public string? CarWashShopName { get; set; }
        public bool NotApproved { get; set; }
    }
}
