using E_come.DTO.DTOAccount;
using E_come.Model;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Identity;

namespace E_come.services
{
    public class UserServices : IUserRepository
    {
       
        public ApplicationUser MapUser(RegisterUserDTO userDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {   FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
     

            };

            return user;
        }
    }
}
