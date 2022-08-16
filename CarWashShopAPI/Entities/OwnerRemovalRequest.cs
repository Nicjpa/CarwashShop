namespace CarWashShopAPI.Entities
{
    public class OwnerRemovalRequest
    {
        public int Id { get; set; }
        public string? RequestStatement { get; set; }
        public string? RequesterId  { get; set; }
        public CustomUser? Requester { get; set; }
        public string? OwnerToBeRemovedId { get; set; }
        public CustomUser? OwnerToBeRemoved { get; set; }
        public int CarWashShopId { get; set; }
        public CarWashShop CarWashShop { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsRemoved { get; set; } = false;
    }
}
