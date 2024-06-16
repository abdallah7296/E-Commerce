using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.DTOAccount
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
