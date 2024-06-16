using E_come.DTO.DTOAccount;
using E_come.DTO.ShopDTO;
using E_come.Model;

namespace E_come.services.IRepository
{
    public interface IShopProductsRepository
    {
        Task<List<ShopDto>> GetAllShop();
        Task<List<ShopDto>> GetMyShop(string userId);
        Task<ShopProducts> GetById(int id);
        Task<ShopProducts> GetByName(string name);
        //Task<AuthModel> Add(ShopUserDto shopDto, IFormFile iamgefil);
        Task<AuthModel> Add(ShopUserDto shopDto, string imageUrl);
        // Task Add(ShopProdDTO shopDto, string userid);
        void DeleteById(int id, string userId);
        void UpdateById(int id, ShopUserUpdateDTO shopDto, string file);
    }
}
