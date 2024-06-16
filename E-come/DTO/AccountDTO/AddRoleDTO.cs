using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.AccountDTO
{
    public class AddRoleDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required] 
        public string RoleName { get; set; }
    }
}
