using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.UserDTOs
{
    public class UserView
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RoleClaim? Role { get; set; }
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
