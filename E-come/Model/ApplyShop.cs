using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_come.Model
{
    public class ApplyShop
    {
        public int Id { get; set; }
       

        public string IdPhoto { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string BusinessPhone { get; set; }

        public string TheDoorNumber { get; set; }

        public string Email { get; set; }


        public string BusinessName { get; set; }

        public string Country { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = " is Requaired ")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Invalid ISBN")]
        public string ISBN { get; set; }


        [ForeignKey("Seller")]
        public string? UserName { get; set; }
        public ApplicationUser? Seller { get; set; }
    }
}
