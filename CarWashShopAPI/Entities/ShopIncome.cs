using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    [Keyless]
    public class ShopIncome
    {
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public int Calendar { get; set; }
        public DateTime Date { get; set; }
        public decimal Income { get; set; }
    }
}
