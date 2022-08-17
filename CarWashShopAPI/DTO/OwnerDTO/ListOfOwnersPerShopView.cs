using Newtonsoft.Json;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class ListOfOwnersPerShopView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Owners { get; set; }
    }
}
