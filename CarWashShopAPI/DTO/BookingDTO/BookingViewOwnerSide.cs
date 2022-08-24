
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingViewOwnerSide
    {
        public int Id { get; set; }
        public string DateCreated { get; set; }
        public string ConsumerId { get; set; }
        public string ConsumerUsername { get; set; }
        public string? Email { get; set; }
        public string? ContactPhone { get; set; }
        public int CarWashShopId { get; set; }
        public string CarWashShopName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ScheduledDate { get; set; }
        public string ScheduledTime { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BookingStatus BookingStatus { get; set; }
    }
}
