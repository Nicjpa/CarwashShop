namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingView
    {
        public int Id { get; set; }
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime ScheduledTime { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
    }
}
