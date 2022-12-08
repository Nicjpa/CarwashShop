namespace CarWashShopAPI.Entities
{
    public class CarWashShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public string Address { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public decimal Revenue { get; set; } = 0;
        public bool isInRemovalProcess { get; set; } = false;
        public List<Transaction> Transactions { get; set; }
        public List<CarWashShopsServices> CarWashShopsServices { get; set; } = new List<CarWashShopsServices>();
        public List<CarWashShopsOwners> Owners { get; set; }
        public List<Booking>? Bookings { get; set; }
        public List<DisbandRequest>? DisbandRequests { get; set; }
        public List<ShopRemovalRequest>? ShopRemovalRequests { get; set; }
        
    }
}
