namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashFilter : PaginationDTO
    {
        public int? CarWashShopId { get; set; }
        public string? CarWashName { get; set; }
        public string? Address { get; set; }
        public string? AdvertisingDescription { get; set; }
        public string? ServiceNameOrDescription { get; set; }
        public int? MinimumAmountOfWashingUnits { get; set; }
        public int? RequiredAndEarlierOpeningTime { get; set; }
        public int? RequiredAndLaterClosingTime { get; set; }
        
    }
}
