using CarWashShopAPI.CustomValidations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingCreation
    {
        [Required]
        public int CarWashShopId { get; set; }
        [Required]
        public int ServiceId { get; set; }

        [DataType(DataType.Date)]
        [DateTimeFormatRequired("yyyy-MM-dd")]
        public string ScheduledDate { get; set; }

        [Required, Range(0, 23)]
        public int ScheduledHour { get; set; }

        [JsonIgnore]
        public DateTime ScheduledDateTime 
        {
            get 
            {
                TimeSpan ts = new TimeSpan(ScheduledHour, 0, 0);
                DateTime.TryParse($"{ScheduledDate} {ts}", out DateTime dateTime);
                return dateTime;
            } 
        }
    }
}
