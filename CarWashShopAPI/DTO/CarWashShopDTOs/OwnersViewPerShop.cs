namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class OwnersViewPerShop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Owners { get; set; }
    }
}
