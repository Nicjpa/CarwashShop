using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarWashShopAPI.DTO
{
    public class Enums
    {
        public enum BookingStatus
        {
            Pending = 1,
            Confirmed = 2,
            Rejected = 3
        }

        public enum CalendarFormat
        {
            Day = 1,
            Week = 2,
            Month = 3,
            Year = 4
        }

        public enum RoleClaim
        {
            Consumer,
            Owner
        }
    }
}
