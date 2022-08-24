namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnerRequestsFilters
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
        public int? CarWashShopId { get; set; }
        public string? CarWashShopName { get; set; }
        public bool NotApproved { get; set; }
    }
}
