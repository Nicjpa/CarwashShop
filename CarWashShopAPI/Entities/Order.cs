namespace CarWashShopAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime ScheduledService { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        
    }
}
