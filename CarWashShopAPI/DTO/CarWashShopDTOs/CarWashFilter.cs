using CarWashShopAPI.DTO.ServiceDTO;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashFilter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int? MinimumAmountOfWashingUnits { get; set; }
        public int? RequiredAndEarlierOpeningTime { get; set; }
        public int? RequiredAndLaterClosingTime { get; set; }
        public string? ServiceNameOrDescription { get; set; }
    }
}
