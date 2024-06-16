using E_come.DTO.DTOAccount;
using E_come.Model;

namespace E_come.services
{
    public interface IUserRepository
    {
        ApplicationUser MapUser(RegisterUserDTO userDTO);

    }
}
