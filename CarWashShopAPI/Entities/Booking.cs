namespace CarWashShopAPI.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ConsumerId { get; set; }
        public CustomUser Consumer { get; set; }
        public int CarWashShopId { get; set; }
        public CarWashShop CarWashShop { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime ScheduledService { get; set; }
        public bool IsPaid { get; set; } = false;
        
    }
}
