using CarWashShopAPI.Entities;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class DisbandRequestView
    {
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public string? RequestStatement { get; set; }
        public string RequesterUserName { get; set; }
        public bool IsApproved { get; set; }
    }
}
