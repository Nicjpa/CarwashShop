namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class FilterServices : PaginationDTO
    {
        public int? ServiceID { get; set; }
        public int? CarWashShopID { get; set; }
        public string? CarWashShopName { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
