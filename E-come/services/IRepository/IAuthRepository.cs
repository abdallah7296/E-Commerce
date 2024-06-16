using E_come.DTO.AccountDTO;
using E_come.DTO.DTOAccount;
using E_come.Model;
using System.IdentityModel.Tokens.Jwt;

namespace E_come.services.IRepository
{
    public interface IAuthRepository
    {
        Task<AuthModel> RegisterAsync(RegisterUserDTO model);
        Task<string> AddRoleAsync(AddRoleDTO model);
        Task<ShowUserDto> GetUser(string userId);
       
    }
}
