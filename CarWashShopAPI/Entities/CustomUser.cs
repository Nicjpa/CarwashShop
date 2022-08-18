using Microsoft.AspNetCore.Identity;

namespace CarWashShopAPI.Entities
{
    public class CustomUser : IdentityUser
    {
        public List<CarWashShopsOwners> CarWashShops { get; set; }
        public List<Booking> Bookings { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ContactPhone { get; set; }
        public string? Picture { get; set; }

    }
}
