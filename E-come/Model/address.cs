using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_come.Model
{
    public class address
    {
        public int Id { get; set; }
        //public string? Address_line { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string region { get; set; }

        [Required(ErrorMessage = "Zip is Required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string zipCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string Unit_number { get; set; }

        [Range(0, 15, ErrorMessage = "Can only be between 0 .. 15")]
        [StringLength(2, ErrorMessage = "Max 2 digits")]
        [Remote("PredictionOK", "Predict", ErrorMessage = "Prediction can only be a number in range 0 .. 15")]
        public string? street_number { get; set; }
        [ForeignKey("user")]

        public string UserName { get; set; }
        public ApplicationUser User  { get; set; }


    }
}
