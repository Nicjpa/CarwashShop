using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnersViewPerShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Owners { get; set; }
    }
}
