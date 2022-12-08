namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class RevenueFilters : PaginationDTO
    {
        public int? ShopID { get; set; }
        public string? ShopName { get; set; }
    }
}
