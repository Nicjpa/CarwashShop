namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class ListOfOwnersPerShopFilters
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 5;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
        public int? CarWashShopId { get; set; }
        public string? CarWashShopName { get; set; }
    }
}
