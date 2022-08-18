using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingViewConsumerSide
    {
        public int Id { get; set; }
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ScheduledDate { get; set; }
        public string ScheduledTime { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
