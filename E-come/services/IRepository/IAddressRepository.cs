using E_come.DTO.AdressDTO;
using E_come.Model;

namespace E_come.services.IRepository
{
    public interface IAddressRepository
    {

        Task<List<address>> GetAllAddressesAsync();
        Task<address> GetById(int id);
        Task<address> GetByName(string name);
        Task Add(AddressDTO category, string UserId);
        void DeleteById(int id);
        void UpdateById(int id, AddressDTO addressDTO);
    }
}
