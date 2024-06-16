using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.ReviewDTO
{
    public class ReviewDTo
    {
        [Required]
        [Range(1, 5, ErrorMessage = "The rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The comment must be at least 10 characters long.")]
        public string Comment { get; set; }
    }
}
