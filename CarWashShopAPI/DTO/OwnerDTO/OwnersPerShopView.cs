using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class OwnersPerShopView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isInRemovalProcess { get; set; }
        public List<string> Owners { get; set; }
    }
}
