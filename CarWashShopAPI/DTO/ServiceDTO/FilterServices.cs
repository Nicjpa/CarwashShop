namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class FilterServices
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 20;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
        public int? ServiceID { get; set; }
        public int? CarWashShopID { get; set; }
        public string? CarWashShopName { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
