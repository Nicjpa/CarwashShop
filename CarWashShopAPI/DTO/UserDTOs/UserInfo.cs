using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.UserDTOs
{
    public class UserInfo
    {
        [Required]
        [MinLength(1), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(1), MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MinLength(9), MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(11), MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(5), MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MinLength(5), MaxLength(20)]
        public string Password { get; set; }

    }
}
