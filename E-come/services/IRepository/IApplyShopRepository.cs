using E_come.DTO.ApplyShopDTO;
using E_come.Model;

namespace E_come.services.IRepository
{
    public interface IApplyShopRepository
    {
        Task<int> Create(ApplayShopDto applayShopDto , IFormFile fromfile,string UserName);
        Task<List<GetApplyDto>> GetAll();
        Task<GetApplyDto> GetById(int id);
        void  DeleteById(int id);

    }
}
