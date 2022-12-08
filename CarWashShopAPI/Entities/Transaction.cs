namespace CarWashShopAPI.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CarWashShopId { get; set; }
        public DateTime PaymentDay { get; set; }
        public decimal Amount { get; set; }
    }
}
