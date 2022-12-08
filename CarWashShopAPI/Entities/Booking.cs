using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string ConsumerId { get; set; }
        public CustomUser Consumer { get; set; }
        public int CarWashShopId { get; set; }
        public CarWashShop CarWashShop { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public bool IsPaid { get; set; } = false;
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public decimal Price { get; set; }
    }
}
