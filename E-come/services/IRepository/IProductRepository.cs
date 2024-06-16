using E_come.DTO.ProductDTO;
using E_come.Model;
using Microsoft.AspNetCore.Mvc;

namespace E_come.services
{
    public interface IProductRepository
    {
        Task<List<productDTO>> GetAll();
        Task<List<productDTO>> GetProducts(int numberOfProducts);
        Task<List<productDTO>> GetMyProduct(string userId);
        Task<List<ProdRev>> GetAllBySortReview();
        Task<productDTO> GetById(int id);
        Task <List<productDTO>> GetByName(string name);
        Task<string> Add( ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles,string userId);
        void DeleteById(int id);
        void UpdateById(int id,ProdWithCategory prodWithCategory, List<IFormFile> ImageFiles);
        Task<int> Create(Product product, List<IFormFile> fromfile);//, string userId);
        Product Mapping(ProdWithCategory prodWithCategory, string userId);

    }
}
