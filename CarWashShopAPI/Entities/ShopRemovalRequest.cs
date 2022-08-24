namespace CarWashShopAPI.Entities
{
    public class ShopRemovalRequest
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public CustomUser Owner { get; set; }
        public int CarWashShopId { get; set; }
        public CarWashShop CarWashShop { get; set; }
        public string? RequestStatement { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
