using Microsoft.AspNetCore.Identity;

namespace CarWashShopAPI.Entities
{
    public class Owner : IdentityUser
    {
        public List<CarWashShopsOwners> CarWashShops { get; set; }

    }
}
