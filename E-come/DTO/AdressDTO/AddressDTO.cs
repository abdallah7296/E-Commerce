using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.AdressDTO
{
    public class AddressDTO
    {
        public string country { get; set; }
        public string city { get; set; }
        public string region { get; set; }

        [Required(ErrorMessage = "Zip is Required")]
 
        public string zipCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone Number Required!")]
        public string phone_number { get; set; }

        [Range(0, 15, ErrorMessage = "Can only be between 0 .. 15")]
        [StringLength(2, ErrorMessage = "Max 2 digits")]
        //   [Remote("PredictionOK", "Predict", ErrorMessage = "Prediction can only be a number in range 0 .. 15")]
        public string? street_number { get; set; }

        // public string UserName { get; set; }
    }
}
