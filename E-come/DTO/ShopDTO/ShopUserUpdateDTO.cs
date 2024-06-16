using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ShopDTO
{
    public class ShopUserUpdateDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string TheDoorNumber { get; set; }
        [Required]

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
