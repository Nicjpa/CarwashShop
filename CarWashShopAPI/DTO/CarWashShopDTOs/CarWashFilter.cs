namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashFilter
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 20;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
        public int? CarWashShopId { get; set; }
        public string? CarWashName { get; set; }
        public string? AdvertisingDescription { get; set; }
        public string? ServiceNameOrDescription { get; set; }
        public int? MinimumAmountOfWashingUnits { get; set; }
        public int? RequiredAndEarlierOpeningTime { get; set; }
        public int? RequiredAndLaterClosingTime { get; set; }
        
    }
}
