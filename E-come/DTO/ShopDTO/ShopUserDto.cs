using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ShopDTO
{
    public class ShopUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string TheDoorNumber { get; set; }
        [Required]

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        //public string?  UserName { get; set; }
        public string? USerId { get; set; }
        [EmailAddress]
        public string? Email {  get; set; }

      
    }
}
