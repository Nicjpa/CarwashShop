using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingCreation
    {
        public int CarWashI { get; set; }
        [Required, Range(1, 24)]
        public int ScheduledHour { get; set; }
    }
}
