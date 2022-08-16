using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.UserDTOs
{
    public class UserInfo
    {
        [Required]
        [MinLength(5), MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Address { get; set; }

    }
}
