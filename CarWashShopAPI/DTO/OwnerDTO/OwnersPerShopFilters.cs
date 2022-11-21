namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnersPerShopFilters : PaginationDTO
    {
        public int? CarWashShopId { get; set; }
        public string? CarWashShopName { get; set; }
    }
}
