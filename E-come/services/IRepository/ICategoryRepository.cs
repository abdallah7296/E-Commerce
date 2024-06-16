using E_come.DTO.CategoryDTO;
using E_come.Model;

namespace E_come.services.IRepository
{
    public interface ICategoryRepository
    {
        List<CategoryDTO> GetAll();
        Task<Category> GetById(int id);
        Task<Category> GetByName(string name);
        void Add(AddCategoryDto category, IFormFile ImageFiles);
        void DeleteById(int id);
        void UpdateById(int id,UpdateCategoryDto category, IFormFile file);
    }
}
