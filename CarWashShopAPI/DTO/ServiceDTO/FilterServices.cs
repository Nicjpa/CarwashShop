namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class FilterServices
    {
        public int? ServiceID { get; set; }
        public string? CarWashShopName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
