namespace CarWashShopAPI.DTO.ServiceDTO
{
    public class FilterServices
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 5;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
        public int? ServiceID { get; set; }
        public string? CarWashShopName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
