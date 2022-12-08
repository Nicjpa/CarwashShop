using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingFilters : PaginationDTO
    {
        public int? BookingID { get; set; }
        public int? CarWashShopID { get; set; }
        public int? ServiceID { get; set; }
        public string? CarWashShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? OnScheduledDate { get; set; }
        public DateTime? ScheduledDatesBefore { get; set; }
        public DateTime? ScheduledDatesAfter { get; set; }
        public int? AtScheduledHour { get; set; }
        public int? ScheduledHoursBefore { get; set; }
        public int? ScheduledHoursAfter { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public bool IsActiveBooking { get; set; }
        public decimal? Price { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

    }
}
