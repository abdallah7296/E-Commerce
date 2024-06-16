using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ApplyShopDTO
{
    public class ApplayShopDto
    {
        

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string BusinessPhone { get; set; }

        public string Email { get; set; }


        public string BusinessName { get; set; }

        public string TheDoorName { get; set; }

        public string Country { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = " is Requaired ")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Invalid ISBN")]
        public string ISBN { get; set; }

    }
}
