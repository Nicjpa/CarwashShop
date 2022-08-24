using System.ComponentModel.DataAnnotations;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.BookingDTO
{
    public class BookingStatusSelection
    {
        [Required]
        public int BookingId { get; set; }
        [Required]
        public BookingStatus BookingStatus { get; set; }
    }
}
