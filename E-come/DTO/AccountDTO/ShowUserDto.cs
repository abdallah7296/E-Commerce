using E_come.DTO.AdressDTO;
using E_come.Model;
using System.ComponentModel.DataAnnotations;

namespace E_come.DTO.AccountDTO
{
    public class ShowUserDto
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string UserName { get; set; }
        public string Phone { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string message { get; set; }
        public bool IsAuthenticated { get; set; }
       public List<string> Roles { get; set; } = new List<string>();
        public List<AddressDTO>? addresses { get; set; }
    }
}
