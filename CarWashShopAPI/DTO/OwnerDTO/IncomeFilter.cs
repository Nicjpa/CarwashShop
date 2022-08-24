using System.ComponentModel.DataAnnotations;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.OwnerDTO
{
    public class IncomeFilter

    {
        [Required]
        public int CarWashShopID { get; set; }
        [Required]
        public CalendarFormat CalendarFormat { get; set; }
        public int RecordsPerSearch { get; set; } = 31;
        public string? StartingDate { get; set; }
        public string? EndingDate { get; set; }
        [Required]
        public string ForTheYear { get; set; }
    }
}
