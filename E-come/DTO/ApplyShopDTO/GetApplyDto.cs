using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ApplyShopDTO
{
    public class GetApplyDto
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string TheDoorNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string BusinessPhone { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = " is Requaired ")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Invalid ISBN")]
        public string ISBN { get; set; }

        public string IDPhoto { get; set; }
       
        //public string userName { get; set;}
        public string userId { get; set;}
    }
}
